using TaskManager_API.Models;

namespace TaskManager_API.Repository;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<bool> UpdateUser(User user); 
    Task<bool> DeleteUser(Guid id);
    Task<User?> GetUser(Guid id);
    Task<User?> GetByName(string name);
    Task<IEnumerable<User>> GetAllUsers();
}