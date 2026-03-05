using Core.DTOs;

namespace Core.Interfaces;

public interface IMessageRepository
{
    public IEnumerable<MessageDTO> GetPublicTimeline();
    public IEnumerable<MessageDTO> GetPublicTimelinePage(int page);
    public IEnumerable<MessageDTO> GetUserTimeline(int user_id);
    public IEnumerable<MessageDTO> GetUserTimelinePage(string username, int page);
    public int GetUserTimelineCount(string username);
    public int GetPublicTimelineCount();
    public IEnumerable<MessageDTO> GetMyTimeline(int userId, int page);
    public void CreateMessage(int author_id, string text);
}