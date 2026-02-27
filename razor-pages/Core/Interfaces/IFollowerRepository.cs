using Core.DTOs;

namespace Core.Interfaces;

public interface IFollowerRepository
{
    public IEnumerable<UserDTO> GetFollowedUsers(int user_id);
    public bool IsFollowed(int source_id, int target_id);
    public void FollowUser(int source_id, int target_id);
    public void UnfollowUser(int source_id, int target_id);
}