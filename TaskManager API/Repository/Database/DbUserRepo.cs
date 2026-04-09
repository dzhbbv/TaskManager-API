using Microsoft.EntityFrameworkCore;
using TaskManager_API.Models;

namespace TaskManager_API.Repository.Database;

public class DbUserRepo(AppDbContext db) : IUserRepository
{
    public async Task<bool> UpdateUser(User user)
    {
        await db.SaveChangesAsync();
        return true;
    }
    
    public async Task<User?> GetUser(Guid id)
    {
        var user = await db.Users.FindAsync(id);
        return user;
    }
    
    public async Task<User?> GetByName(string name)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Name == name);
        return user;
    }
    
    public async Task<IEnumerable<User>> GetAllUsers() => await db.Users.ToListAsync();
    
    public async Task<bool> DeleteUser(Guid id)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null) return false;
        
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return true;
    }
    
    public async Task AddUser(User user)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
    }
}