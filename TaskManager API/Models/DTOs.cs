namespace TaskManager_API.Models;

// То что киент присылает серверу при создании TodoTask
public record TodoTaskDto(string Title, string Description);

// То что клиент получает по GET-запросу
public record TodoTaskResponseDTO(Guid Id, string Title, string Description, TaskStatus Status, DateTime CreatedAt); 

// Обновленная задача от пользователя
public record UpdateTodoTaskDto(string Title, string Description, TaskStatus Status);

public record UserDto(string Name, string Password);

public record UserResponseDto(Guid Id, string Name);