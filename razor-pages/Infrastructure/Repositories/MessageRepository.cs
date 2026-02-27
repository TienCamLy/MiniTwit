using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly MiniTwitContext _context;
    private const int _messagesPerPage = 30;
    public MessageRepository(MiniTwitContext context)
    {
        _context = context;
    }

    public IEnumerable<MessageDTO> GetPublicTimeline()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MessageDTO> GetUserTimeline(int user_id)
    {
        throw new NotImplementedException();
    }
    
    public IEnumerable<MessageDTO> GetUserTimeline(string username)
    {
        throw new NotImplementedException();
    }

    public void CreateMessage(int author_id, string text)
    {
        throw new NotImplementedException();
    }
}