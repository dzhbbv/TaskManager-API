using System.Collections.Concurrent;
using TaskManager_API.Models;

namespace TaskManager_API.Repository;

public class InMemoryUserRepo(ConcurrentDictionary<Guid, User> users) : IUserRepository
{
    public IEnumerable<User> GetAllUsers() => users.Values;
    
    public bool UpdateUser(Guid id, User user)
    {
        if (!users.TryGetValue(id, out var oldUser))
            return false;
        return users.TryUpdate(id, user, oldUser);
    }

    public User AddUser(User user)
    {
        users.TryAdd(user.Id, user);
        return user;
    }
    
    public bool DeleteUser(Guid id) => users.TryRemove(id, out _);
    
    public User? GetUser(Guid id) => users.GetValueOrDefault(id);

    public User? GetByName(string name) => users.Values.FirstOrDefault(u => u.Name == name);
}