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
    /// <summary>Singleton row <c>Id = 1</c>; persisted <c>latest_id</c> for the simulator API.</summary>
    public DbSet<SimulatorLatest> SimulatorLatestState => Set<SimulatorLatest>();
    
    public MiniTwitContext(DbContextOptions<MiniTwitContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Follower>()
            .HasKey(f => new { f.SourceId, f.TargetId });

        modelBuilder.Entity<SimulatorLatest>(e =>
        {
            e.ToTable("SimulatorLatest");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedNever();
            e.Property(x => x.LatestId).HasColumnName("latest_id");
            e.HasData(new SimulatorLatest { Id = SimulatorLatest.SingletonRowId, LatestId = 0 });
        });
    }
}