using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Context;

public class MiniTwitContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    // public DbSet<User> Users => Set<User>(); (implicit)
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Follower> Followers => Set<Follower>();
    
    public MiniTwitContext(DbContextOptions<MiniTwitContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Follower>()
            .HasKey(f => new { f.SourceId, f.TargetId });
    }
}