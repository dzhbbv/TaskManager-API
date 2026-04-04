using TaskManager_API.Models;
using TaskManager_API.Repository;
namespace TaskManager_API.Services;

public class UserRepoService(IUserRepository users, ITaskRepository tasks) : IUserRepoService
{
    public UserResponseDto? CreateUser(UserDto userDto)
    {
        var existing = users.GetByName(userDto.Name);
        if (existing is not null) return null;
        var hash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var user = new User(Guid.NewGuid(), userDto.Name, hash);
        users.AddUser(user);
        return new UserResponseDto(user.Id,user.Name);
    }
    
    public bool DeleteUser(Guid id)
    {
        var user = users.GetUser(id);
        if (user == null) return false;

        var userTaskIds = tasks.GetAllTasks()
            .Where(t => t.UserId == id)
            .Select(t => t.Id)
            .ToList();
        
        foreach (var taskId in userTaskIds)
        {
            tasks.DeleteTask(taskId);
        }
        
        return users.DeleteUser(id);
    }
    
    public bool UpdateUser(Guid id, UserResponseDto userDto)
    {
        var user = users.GetUser(id);
        if (user is null) return false;
        var newUser = user  with { Name = userDto.Name };
        return users.UpdateUser(id, newUser);
    }

    public UserResponseDto? GetUserById(Guid id)
    {
        var user = users.GetUser(id);
        if (user is null) return null; 
        return new UserResponseDto(user.Id, user.Name);
    }

    public User? GetUserByName(string name) => users.GetByName(name);
    
    public IEnumerable<UserResponseDto> GetUsers() => users.GetAllUsers().Select(user => new UserResponseDto(user.Id, user.Name));
}

public interface IUserRepoService
{
    UserResponseDto? CreateUser(UserDto userDto);
    bool DeleteUser(Guid id);
    bool UpdateUser(Guid id, UserResponseDto userDto);
    UserResponseDto? GetUserById(Guid id);
    User? GetUserByName(string name);
    IEnumerable<UserResponseDto> GetUsers();
}