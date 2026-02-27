using Core.DTOs;

namespace Core.Interfaces;

public interface IMessageRepository
{
    public IEnumerable<MessageDTO> GetPublicTimeline();
    public IEnumerable<MessageDTO> GetUserTimeline(int user_id);
    public void CreateMessage(int author_id, string text);
}