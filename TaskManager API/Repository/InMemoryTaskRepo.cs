using System.Collections.Concurrent;
using TaskManager_API.Models;

namespace TaskManager_API.Repository;

public class InMemoryTaskRepo(ConcurrentDictionary<Guid, TodoTask> tasks) : ITaskRepository
{
    public IEnumerable<TodoTask> GetAllTasks() => tasks.Values;

    public bool UpdateTask(Guid id, TodoTask task)
    {
        if (!tasks.TryGetValue(id, out var oldTask))
            return false;
        return tasks.TryUpdate(id, task, oldTask);
    }

    public TodoTask AddTask(TodoTask task)
    {
        tasks.TryAdd(task.Id, task);
        return task;
    }

    public bool DeleteTask(Guid id) => tasks.TryRemove(id, out _);

    public TodoTask? GetTaskById(Guid id) => tasks.GetValueOrDefault(id);
}