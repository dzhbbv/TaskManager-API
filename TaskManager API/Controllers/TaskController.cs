using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Models;
using TaskManager_API.Services;

namespace TaskManager_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskRepoService taskRepo, ILogger<TaskController> logger) : ControllerBase
{
    private Guid CurrentUserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                                             ?? throw new UnauthorizedAccessException());

    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] TodoTaskDto dto) 
    {
        var result = await taskRepo.CreateTask(dto, CurrentUserId);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        if (!await taskRepo.DeleteTask(id, CurrentUserId)) 
            return BadRequest("Task not found");
            
        logger.LogInformation($"Task {id} was deleted");
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTodoTaskDto update)
    {
        if (!await taskRepo.UpdateTask(id, update, CurrentUserId)) 
            return BadRequest("Task not found");
            
        logger.LogInformation($"Task {id} was updated ({update.Title})");
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        var task = await taskRepo.GetTask(id, CurrentUserId);
        return task is null ? NotFound("Task not found or access denied") : Ok(task);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll() 
    {
        var tasks = await taskRepo.GetUserTasks(CurrentUserId);
        return Ok(tasks);
    }
}