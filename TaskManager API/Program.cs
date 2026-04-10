using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TaskManager_API.Repository;
using TaskManager_API.Repository.Database;
// using TaskManager_API.Repository.InMemory;
using TaskManager_API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUserRepository, DbUserRepo>();
builder.Services.AddScoped<ITaskRepository, DbTaskRepo>();
// Memory
// builder.Services.AddSingleton<ConcurrentDictionary<Guid, User>>();
// builder.Services.AddSingleton<ConcurrentDictionary<Guid, TodoTask>>();

// builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepo>();
// builder.Services.AddSingleton<IUserRepository, InMemoryUserRepo>();

// Services
builder.Services.AddScoped<IUserRepoService, UserRepoService>();
builder.Services.AddScoped<ITaskRepoService, TaskRepoService>();

builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["JwtSettings:Key"] 
                 ?? throw new InvalidOperationException("JWT Key is missing in config");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
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