using Microsoft.EntityFrameworkCore;
using TaskManager_API.Models;

namespace TaskManager_API.Repository.Database;

public class DbTaskRepo(AppDbContext db) : ITaskRepository
{
    public async Task<IEnumerable<TodoTask>> GetAllTasks() => await db.TodoTasks.ToListAsync();

    public async Task<bool> UpdateTask(TodoTask task)
    {
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<TodoTask> AddTask(TodoTask task)
    {
        await db.TodoTasks.AddAsync(task);
        await db.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTask(Guid id)
    {
        var task = await db.TodoTasks.FindAsync(id);
        if (task == null) return false;
        
        db.TodoTasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<TodoTask?> GetTaskById(Guid id) => await db.TodoTasks.FindAsync(id);

}

