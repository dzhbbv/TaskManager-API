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
    public IActionResult AddTask([FromBody] TodoTaskDto dto) => Ok(taskRepo.CreateTask(dto, CurrentUserId));
    
    [HttpDelete("{id}")]
    public IActionResult DeleteTask(Guid id)
    {
        if (!taskRepo.DeleteTask(id, CurrentUserId)) return BadRequest("Task not found");
        logger.LogInformation($"Task {id} was deleted");
        return Ok();
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateTask(Guid id, [FromBody] UpdateTodoTaskDto update)
    {
        if (!taskRepo.UpdateTask(id, update, CurrentUserId)) return BadRequest("Task not found");
        logger.LogInformation($"Task {id} was updated ({update.Title})");
        return Ok();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetTask(Guid id)
    {
        var task = taskRepo.GetTask(id, CurrentUserId);
        return task is null ? NotFound("Task not found or access denied") : Ok(task);
    }
    
    [HttpGet]
    public IActionResult GetAll() => Ok(taskRepo.GetUserTasks(CurrentUserId));
}