using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Models;
using TaskManager_API.Services;

namespace TaskManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ILogger<UserController> logger, IUserRepoService userRepo, IAuthService authService)
    : ControllerBase
{
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] string newName) // async Task
    {
        // Добавляем await
        var success = await userRepo.UpdateUser(id, new UserResponseDto(id, newName));
        if (!success) return NotFound("User not found");
        logger.LogInformation($"User {id} updated to {newName}");
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) // async Task
    {
        if (!await userRepo.DeleteUser(id)) return NotFound(); // await
        logger.LogInformation($"User {id} deleted");
        return Ok();
    }
    
    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers() => Ok(await userRepo.GetUsers()); // await
    
    [Authorize]
    [HttpGet("byName/{name}")]
    public async Task<IActionResult> GetUserByName(string name) // async Task
    {
        var user = await userRepo.GetUserByName(name); // await
        if (user is null) return BadRequest("User not found");
        
        var response = await userRepo.GetUserById(user.Id); // await
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto) // async Task
    {
        var user = await userRepo.GetUserByName(userDto.Name); // await
        
        // authService.VerifyPassword обычно синхронный (просто хэш чекает), его можно без await
        if (user is null || !authService.VerifyPassword(userDto.Password, user.PasswordHash))
        { 
            logger.LogInformation($"Fucking slave {userDto.Name} login failed");
            return Unauthorized("Invalid credentials");
        }

        logger.LogInformation($"User {userDto.Name} logged in");
        return Ok(new { AccessToken = authService.CreateToken(user) });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto) // async Task
    {
        var response = await userRepo.CreateUser(userDto); // await
        if (response is null) return BadRequest("User already exists");
        
        logger.LogInformation($"User {userDto.Name} registered");
        return Ok(response);
    }
}