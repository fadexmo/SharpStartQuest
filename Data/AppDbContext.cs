using Microsoft.EntityFrameworkCore;
using SharpStartQuest.Models;

namespace SharpStartQuest.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Quest> Quests => Set<Quest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quest>()
            .Property(quest => quest.Title)
            .HasMaxLength(90);

        modelBuilder.Entity<Quest>()
            .Property(quest => quest.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Quest>()
            .HasIndex(quest => quest.IsCompleted);

        modelBuilder.Entity<Quest>()
            .HasIndex(quest => quest.Category);
    }
}
