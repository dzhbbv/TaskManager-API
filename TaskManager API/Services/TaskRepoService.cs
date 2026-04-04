using TaskManager_API.Models;
using TaskManager_API.Repository;

namespace TaskManager_API.Services;

public class TaskRepoService(ITaskRepository repo) : ITaskRepoService
{
    public IEnumerable<TodoTaskResponseDTO> GetUserTasks(Guid ownerId) => 
        repo.GetAllTasks().Where(u=>u.UserId == ownerId)
            .Select(u=> new TodoTaskResponseDTO(u.Id, u.Title, u.Description, u.Status, u.CreatedAt));
    
    public TodoTaskResponseDTO? GetTask(Guid id, Guid ownerId)
    {
        var t = repo.GetTaskById(id);
        if (t is null || t.UserId != ownerId) return null;
        
        return new TodoTaskResponseDTO(t.Id, t.Title, t.Description, t.Status, t.CreatedAt);
    }
    
    public bool UpdateTask(Guid id, UpdateTodoTaskDto taskDto, Guid ownerId)
    {
        var existing = repo.GetTaskById(id);
        if (existing is null || existing.UserId != ownerId) return false;
        
        var updatedTask = existing with
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            Status = taskDto.Status 
        };
        
        return repo.UpdateTask(id, updatedTask);
    }
    
    public bool DeleteTask(Guid id, Guid ownerId)
    {
        var existing = GetTask(id, ownerId);
        if (existing is null) return false;
        
        return repo.DeleteTask(id);
    }
    
    public TodoTaskResponseDTO CreateTask(TodoTaskDto taskDto, Guid ownerId)
    {
        var task = new TodoTask(
            Id: Guid.NewGuid(),
            Title: taskDto.Title,
            Description: taskDto.Description,
            UserId: ownerId,
            Status: Models.TaskStatus.New,
            CreatedAt: DateTime.UtcNow
        );
        repo.AddTask(task);
        var response = new TodoTaskResponseDTO(task.Id, task.Title, task.Description, task.Status, task.CreatedAt);
        return response;
    }
}

public interface ITaskRepoService
{
    TodoTaskResponseDTO CreateTask(TodoTaskDto taskDto, Guid ownerId);
    
    bool DeleteTask(Guid id, Guid ownerId);
    
    bool UpdateTask(Guid id, UpdateTodoTaskDto taskDto, Guid ownerId);
    
    TodoTaskResponseDTO? GetTask(Guid id, Guid ownerId);
    
    IEnumerable<TodoTaskResponseDTO> GetUserTasks(Guid ownerId);
}