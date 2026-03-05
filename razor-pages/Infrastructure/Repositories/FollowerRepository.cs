using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class FollowerRepository : IFollowerRepository
{
    private readonly MiniTwitContext _context;
    public FollowerRepository(MiniTwitContext context)
    {
        _context = context;
    }

    public IEnumerable<UserDTO> GetFollowedUsers(int userId)
    {
        var followedUsers = _context.Followers
            .Where(f => f.SourceId == userId)
            .Select(f => f.Target)
            .Select<User, UserDTO>(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
            })
            .ToList();

        return followedUsers;
    }

    public bool IsFollowed(int sourceId, int targetId)
    {
        return _context.Followers.Any(f => f.SourceId == sourceId && f.TargetId == targetId);
    }

    public void FollowUser(int sourceId, int targetId)
    {
        var sourceUser = _context.Users.SingleOrDefault(u => u.Id == sourceId);
        var targetUser = _context.Users.SingleOrDefault(u => u.Id == targetId);
        
        if (sourceUser == null || targetUser == null) throw new Exception("User not found");
        
        var alreadyFollowing = _context.Followers.Any(f => f.SourceId == sourceId && f.TargetId == targetId);
        if (alreadyFollowing) return;

        var follower = new Follower
        {
            Source = sourceUser,
            SourceId = sourceId,
            Target = targetUser,
            TargetId = targetId
        };
        _context.Followers.Add(follower);
        _context.SaveChanges();
    }

    public void UnfollowUser(int sourceId, int targetId)
    {
        var sourceUser = _context.Users.SingleOrDefault(u => u.Id == sourceId);
        var targetUser = _context.Users.SingleOrDefault(u => u.Id == targetId);
        
        if (sourceUser == null || targetUser == null) throw new Exception("User not found");
        
        var alreadyFollowing = _context.Followers.Any(f => f.SourceId == sourceId && f.TargetId == targetId);
        if (!alreadyFollowing) return;

        var follower = new Follower
        {
            Source = sourceUser,
            SourceId = sourceId,
            Target = targetUser,
            TargetId = targetId
        };
        _context.Followers.Remove(follower);
        _context.SaveChanges();
    }
}