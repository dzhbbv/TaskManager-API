using TaskManager_API.Models;

namespace TaskManager_API.Repository;

public interface IUserRepository
{
    User AddUser(User user);
    bool UpdateUser(Guid id, User user); 
    bool DeleteUser(Guid id);
    User? GetUser(Guid id);
    User? GetByName(string name);
    IEnumerable<User> GetAllUsers();
}