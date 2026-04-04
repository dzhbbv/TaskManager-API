using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TaskManager_API.Models;
namespace TaskManager_API.Services;
using BCrypt.Net;

public class AuthService : IAuthService
{
    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey("Three_hundred_backs@GymBoss-Semen"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: "DungeonMaster",
            audience: "Client",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Verify(password, hash);
    }
}

public interface IAuthService
{
    string CreateToken(User user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}