using razor_pages.Structs;
namespace razor_pages.Pages;

public interface IDBContext
{
    User GetUserById(string id);
    User GetUserByUsername(string username);
    List<Message> GetPublicTimeline(int perPage);
    List<Message> GetUserTimeline(int perPage, string username);
    List<Message> GetOwnTimeline(int perPage, int author_id);
    void CreateUser(string username, string email, string passwordHash);
    void CreateMessage(int authorId, string text);
    List<string> GetFollowedUsers(int whoId, int maxResults);
    bool IsFollowed(int whoId, int whomId);
    void FollowUser(int whoId, int whomId);
    void UnfollowUser(int whoId, int whomId);
    User? Login(string username, string passwordHash);
}