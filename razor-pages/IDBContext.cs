using razor_pages.Structs;
namespace razor_pages.Pages;

public interface IDBContext
{
    User GetUserById(string id);
    List<Message> GetPublicTimeline(int perPage);
}