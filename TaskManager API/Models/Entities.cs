namespace TaskManager_API.Models;

public class TodoTask
{
    // Пустой конструктор для EF Core
    private TodoTask() { } 

    // Твой текущий конструктор
    public TodoTask(Guid id, string title, string description, Guid userId, TaskStatus status, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        UserId = userId;
        Status = status;
        CreatedAt = createdAt;
    }

    public Guid Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid UserId { get; init; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; init; }
}

public class User
{
    private User() { }

    public User(Guid id, string name, string passwordHash)
    {
        Id = id;
        Name = name;
        PasswordHash = passwordHash;
    }

    public Guid Id { get; init; }
    public string Name { get; set; }
    public string PasswordHash { get; init; }
}

public enum TaskStatus {New, InProgress, Done, Canceled}