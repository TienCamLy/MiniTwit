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

    public IEnumerable<UserDTO> GetFollowedUsers(int user_id)
    {
        var followedUsers = _context.Followers
            .Where(f => f.SourceId == user_id)
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

    public bool IsFollowed(int source_id, int target_id)
    {
        return _context.Followers.Any(f => f.SourceId == source_id && f.TargetId == target_id);
    }

    public void FollowUser(int source_id, int target_id)
    {
        throw new NotImplementedException();
    }

    public void UnfollowUser(int source_id, int target_id)
    {
        throw new NotImplementedException();
    }
}