using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TaskManager_API.Models;
using TaskManager_API.Repository;
using TaskManager_API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Memory
builder.Services.AddSingleton<ConcurrentDictionary<Guid, User>>();
builder.Services.AddSingleton<ConcurrentDictionary<Guid, TodoTask>>();
// Repositories
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepo>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepo>();
// Services
builder.Services.AddScoped<IUserRepoService, UserRepoService>();
builder.Services.AddScoped<ITaskRepoService, TaskRepoService>();

builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "DungeonMaster",
        ValidAudience = "Client",
        IssuerSigningKey = new SymmetricSecurityKey("Three_hundred_backs@GymBoss-Semen"u8.ToArray())
    };
});
var app = builder.Build();

// --- СЕКЦИЯ MIDDLEWARE ---
// 1. Генерируем сам документ (чертеж API) по адресу /openapi/v1.json
app.MapOpenApi(); 
// 2. Подключаем Scalar (визуализатор), который читает этот чертеж
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();
// 3. Авторизация и аутентификация
app.UseAuthentication();
app.UseAuthorization();
// 4. Маппим контроллеры
app.MapControllers();

app.Run();