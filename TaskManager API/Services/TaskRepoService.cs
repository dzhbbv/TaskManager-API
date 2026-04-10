using TaskManager_API.Models;
using TaskManager_API.Repository;
using TaskStatus = TaskManager_API.Models.TaskStatus;

namespace TaskManager_API.Services;

public class TaskRepoService(ITaskRepository repo) : ITaskRepoService
{
    public async Task<IEnumerable<TodoTaskResponseDto>> GetUserTasks(Guid ownerId)
    {
        var tasks = await repo.GetAllTasks();
        return tasks.Where(u => u.UserId == ownerId)
                    .Select(u => new TodoTaskResponseDto(u.Id, u.Title, u.Description, u.Status, u.CreatedAt));
    }
    
    public async Task<TodoTaskResponseDto?> GetTask(Guid id, Guid ownerId)
    {
        var t = await repo.GetTaskById(id);
        if (t is null || t.UserId != ownerId) return null;
        return new TodoTaskResponseDto(t.Id, t.Title, t.Description, t.Status, t.CreatedAt);
    }
    
    public async Task<bool> UpdateTask(Guid id, UpdateTodoTaskDto taskDto, Guid ownerId)
    {
        var existing = await repo.GetTaskById(id);
        if (existing is null || existing.UserId != ownerId) return false;
        
        if (!string.IsNullOrWhiteSpace(taskDto.Title)) existing.Title = taskDto.Title;
        if (!string.IsNullOrWhiteSpace(taskDto.Description)) existing.Description = taskDto.Description;

        existing.Status = taskDto.Status;
        
        return await repo.UpdateTask(existing);
    }
    
    public async Task<bool> DeleteTask(Guid id, Guid ownerId)
    {
        var existing = await repo.GetTaskById(id);
        if (existing is null || existing.UserId != ownerId) return false;
        
        return await repo.DeleteTask(id);
    }
    
    public async Task<TodoTaskResponseDto> CreateTask(TodoTaskDto taskDto, Guid ownerId)
    {
        var task = new TodoTask(
            Guid.NewGuid(),
            taskDto.Title,
            taskDto.Description,
            ownerId,
            TaskStatus.New,
            DateTime.UtcNow
        );
        
        await repo.AddTask(task);
        
        return new TodoTaskResponseDto(task.Id, task.Title, task.Description, task.Status, task.CreatedAt);
    }
}

public interface ITaskRepoService
{
    Task<TodoTaskResponseDto> CreateTask(TodoTaskDto taskDto, Guid ownerId);
    
    Task<bool> DeleteTask(Guid id, Guid ownerId);
    
    Task<bool> UpdateTask(Guid id, UpdateTodoTaskDto taskDto, Guid ownerId);
    
    Task<TodoTaskResponseDto?> GetTask(Guid id, Guid ownerId);
    
    Task<IEnumerable<TodoTaskResponseDto>> GetUserTasks(Guid ownerId);
}