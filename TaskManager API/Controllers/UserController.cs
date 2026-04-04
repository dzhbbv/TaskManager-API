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
    public IActionResult Update(Guid id, [FromBody] string newName)
    {
        var success = userRepo.UpdateUser(id, new UserResponseDto(id, newName));
        if (!success) return NotFound("User not found");
        logger.LogInformation($"User {id} updated to {newName}");
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        if (!userRepo.DeleteUser(id)) return NotFound();
        logger.LogInformation($"User {id} deleted");
        return Ok();
    }
    
    [Authorize]
    [HttpGet("users")]
    public IActionResult GetUsers() => Ok(userRepo.GetUsers());
    
    [Authorize]
    [HttpGet("byName/{name}")]
    public IActionResult GetUserByName(string name)
    {
        var user = userRepo.GetUserByName(name);
        if (user is null) return BadRequest("User not found");
        var response = userRepo.GetUserById(user.Id);
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet("byId/{id}")]
    public IActionResult GetUserById(Guid id)
    {
        var response = userRepo.GetUserById(id);
        if (response is null) return BadRequest("User not found");
        return Ok(response);
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
        var user = userRepo.GetUserByName(userDto.Name);
        if (user is null || !authService.VerifyPassword(userDto.Password, user.PasswordHash))
        { 
            logger.LogInformation($"Fucking slave {userDto.Name} login failed (For test: password is {userDto.Password})");
            return Unauthorized("Invalid credentials");
        }

        logger.LogInformation($"User {userDto.Name} logged in");
        return Ok(new { AccessToken = authService.CreateToken(user) });
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
        var response = userRepo.CreateUser(userDto);
        if (response is null) return BadRequest("User already exists");
        logger.LogInformation($"User {userDto.Name} registered (For test: password is {userDto.Password})");
        return Ok(response);
    }
}