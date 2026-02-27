using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Context;

public class MiniTwitContext : IdentityDbContext<User>
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Follower> Followers => Set<Follower>();
    
    public MiniTwitContext(DbContextOptions<MiniTwitContext> options) : base(options)
    {
    }
}