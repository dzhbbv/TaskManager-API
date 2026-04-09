using Microsoft.EntityFrameworkCore;
using TaskManager_API.Models;

namespace TaskManager_API.Repository;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
    public DbSet<User> Users => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoTask>()
            .Property(t => t.Title)
            .HasMaxLength(100)
            .IsRequired(); 

        modelBuilder.Entity<TodoTask>()
            .Property(t => t.Description)
            .HasMaxLength(1000);
        
        modelBuilder.Entity<TodoTask>()
            .HasOne<User>()             
            .WithMany()           
            .HasForeignKey(t => t.UserId) 
            .OnDelete(DeleteBehavior.Cascade);
            
        base.OnModelCreating(modelBuilder);
    }
}
