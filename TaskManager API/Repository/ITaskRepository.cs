using TaskManager_API.Models;

namespace TaskManager_API.Repository;

public interface ITaskRepository
{
    TodoTask AddTask(TodoTask task);
    bool UpdateTask(Guid id, TodoTask task);
    bool DeleteTask(Guid id);
    TodoTask? GetTaskById(Guid id);
    IEnumerable<TodoTask> GetAllTasks();
}