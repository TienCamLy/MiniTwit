using Core.DTOs;

namespace Core.Interfaces;

public interface IFollowerRepository
{
    public IEnumerable<UserDTO> GetFollowedUsers(int userId);
    public bool IsFollowed(int sourceId, int targetId);
    public void FollowUser(int sourceId, int targetId);
    public void UnfollowUser(int sourceId, int targetId);
}