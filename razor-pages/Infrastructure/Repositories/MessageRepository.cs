using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    // TODO: Take page number into account, currently only the first page is returned
    private readonly MiniTwitContext _context;
    private const int _messagesPerPage = 30;
    public MessageRepository(MiniTwitContext context)
    {
        _context = context;
    }

    public IEnumerable<MessageDTO> GetPublicTimeline()
    {
        return GetMessages().Take(_messagesPerPage);
    }

    public IEnumerable<MessageDTO> GetUserTimeline(int user_id)
    {
        return GetMessages().Where(m => m.author_id == user_id).Take(_messagesPerPage);
    }
    
    public IEnumerable<MessageDTO> GetUserTimeline(string username)
    {
        return GetMessages().Where(m => m.author_name == username).Take(_messagesPerPage);
    }

    public void CreateMessage(int author_id, string text)
    {
        var user = _context.Users.Where(u => u.Id == author_id).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var message = new Message
        {
            author_id = author_id,
            author_name = user.UserName,
            author_email = user.Email,
            text = text,
            pub_date = DateTime.UtcNow,
            flagged = "false"
        };
        
        _context.Messages.Add(message);
        _context.SaveChanges();
    }

    private IEnumerable<MessageDTO> GetMessages()
    {
        var messages = _context.Messages
            .OrderByDescending(m => m.pub_date)
            .Select(m => new MessageDTO
            {
                id = m.id,
                author_id = m.author_id,
                author_name = m.author_name,
                author_email = m.author_email,
                text = m.text,
                pub_date = m.pub_date.ToString("yyyy-MM-dd HH:mm:ss")
            })
            .ToList();
        
        return messages.Take(_messagesPerPage);
    }
}