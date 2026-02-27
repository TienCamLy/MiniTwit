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
        throw new NotImplementedException();
    }

    public bool IsFollowed(int source_id, int target_id)
    {
        throw new NotImplementedException();
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