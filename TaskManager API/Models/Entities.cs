namespace TaskManager_API.Models;

public record TodoTask(Guid Id, string Title, string Description, Guid UserId, TaskStatus Status, DateTime CreatedAt);

public record User(Guid Id, string Name, string PasswordHash);

public enum TaskStatus {New, InProgress, Done, Canceled}