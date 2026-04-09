using TaskManager_API.Models;

namespace TaskManager_API.Repository;

public interface ITaskRepository
{
    Task<TodoTask> AddTask(TodoTask task);
    Task<bool> UpdateTask(TodoTask task);
    Task<bool> DeleteTask(Guid id);
    Task<TodoTask?> GetTaskById(Guid id);
    Task<IEnumerable<TodoTask>> GetAllTasks();
}