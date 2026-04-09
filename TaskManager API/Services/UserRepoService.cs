using TaskManager_API.Models;
using TaskManager_API.Repository;
namespace TaskManager_API.Services;

public class UserRepoService(IUserRepository users) : IUserRepoService
{
    public async Task<UserResponseDto?> CreateUser(UserDto userDto)
    {
        var existing = await users.GetByName(userDto.Name);
        if (existing is not null) return null;

        var hash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var user = new User(Guid.NewGuid(), userDto.Name, hash);
        
        await users.AddUser(user);
        return new UserResponseDto(user.Id, user.Name);
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        return await users.DeleteUser(id);
    }

    public async Task<bool> UpdateUser(Guid id, UserResponseDto userDto)
    {
        var user = await users.GetUser(id);
        if (user is null) return false;
        
        user.Name = userDto.Name;

        return await users.UpdateUser(user);
    }

    public async Task<UserResponseDto?> GetUserById(Guid id)
    {
        var user = await users.GetUser(id);
        if (user is null) return null;
        return new UserResponseDto(user.Id, user.Name);
    }

    public async Task<User?> GetUserByName(string name) => await users.GetByName(name);

    public async Task<IEnumerable<UserResponseDto>> GetUsers()
    {
        var allUsers = await users.GetAllUsers();
        return allUsers.Select(user => new UserResponseDto(user.Id, user.Name));
    }
}

public interface IUserRepoService
{
    Task<UserResponseDto?> CreateUser(UserDto userDto);
    Task<bool> DeleteUser(Guid id);
    Task<bool> UpdateUser(Guid id, UserResponseDto userDto);
    Task<UserResponseDto?> GetUserById(Guid id);
    Task<User?> GetUserByName(string name);
    Task<IEnumerable<UserResponseDto>> GetUsers();
}