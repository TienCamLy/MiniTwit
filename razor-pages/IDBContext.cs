using razor_pages.Structs;
namespace razor_pages.Pages;

public interface IDBContext
{
    User GetUserById(string id);
    User GetUserByUsername(string username);
    List<Message> GetPublicTimeline(int perPage);
    List<Message> GetUserTimeline(int perPage, string username);
    void CreateUser(string username, string email, string passwordHash);
    void CreateMessage(int author_id, string text);
    bool IsFollowed(int whoId, int whomId);
    void FollowUser(int whoId, int whomId);
    void UnfollowUser(int whoId, int whomId);
    User? Login(string username, string passwordHash);
}