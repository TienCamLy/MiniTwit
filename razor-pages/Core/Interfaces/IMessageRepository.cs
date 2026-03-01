using Core.DTOs;

namespace Core.Interfaces;

public interface IMessageRepository
{
    public IEnumerable<MessageDTO> GetPublicTimeline(int page);
    public IEnumerable<MessageDTO> GetUserTimeline(int user_id);
    public IEnumerable<MessageDTO> GetUserTimeline(string username, int page);
    public int GetUserTimelineCount(string username);
    public int GetPublicTimelineCount();
    public void CreateMessage(int author_id, string text);
}