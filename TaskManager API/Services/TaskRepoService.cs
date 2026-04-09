using TaskManager_API.Models;
using TaskManager_API.Repository;

namespace TaskManager_API.Services;

public class TaskRepoService(ITaskRepository repo) : ITaskRepoService
{
    public async Task<IEnumerable<TodoTaskResponseDTO>> GetUserTasks(Guid ownerId)
    {
        var tasks = await repo.GetAllTasks();
        return tasks.Where(u => u.UserId == ownerId)
                    .Select(u => new TodoTaskResponseDTO(u.Id, u.Title, u.Description, u.Status, u.CreatedAt));
    }
    
    public async Task<TodoTaskResponseDTO?> GetTask(Guid id, Guid ownerId)
    {
        var t = await repo.GetTaskById(id);
        if (t is null || t.UserId != ownerId) return null;
        
        return new TodoTaskResponseDTO(t.Id, t.Title, t.Description, t.Status, t.CreatedAt);
    }
    
    public async Task<bool> UpdateTask(Guid id, UpdateTodoTaskDto taskDto, Guid ownerId)
    {
        var existing = await repo.GetTaskById(id);
        if (existing is null || existing.UserId != ownerId) return false;
        
        existing.Title = taskDto.Title;
        existing.Description = taskDto.Description;
        existing.Status = taskDto.Status;
        
        return await repo.UpdateTask(existing);
    }
    
    public async Task<bool> DeleteTask(Guid id, Guid ownerId)
    {
        var existing = await repo.GetTaskById(id);
        if (existing is null || existing.UserId != ownerId) return false;
        
        return await repo.DeleteTask(id);
    }
    
    public async Task<TodoTaskResponseDTO> CreateTask(TodoTaskDto taskDto, Guid ownerId)
    {
        var task = new TodoTask(
            Guid.NewGuid(),
            taskDto.Title,
            taskDto.Description,
            ownerId,
            Models.TaskStatus.New,
            DateTime.UtcNow
        );
        
        await repo.AddTask(task);
        
        return new TodoTaskResponseDTO(task.Id, task.Title, task.Description, task.Status, task.CreatedAt);
    }
}

public interface ITaskRepoService
{
    Task<TodoTaskResponseDTO> CreateTask(TodoTaskDto taskDto, Guid ownerId);
    
    Task<bool> DeleteTask(Guid id, Guid ownerId);
    
    Task<bool> UpdateTask(Guid id, UpdateTodoTaskDto taskDto, Guid ownerId);
    
    Task<TodoTaskResponseDTO?> GetTask(Guid id, Guid ownerId);
    
    Task<IEnumerable<TodoTaskResponseDTO>> GetUserTasks(Guid ownerId);
}