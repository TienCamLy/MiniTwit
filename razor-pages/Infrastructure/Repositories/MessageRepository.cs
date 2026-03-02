using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly MiniTwitContext _context;
    private const int _messagesPerPage = 10;
    public MessageRepository(MiniTwitContext context)
    {
        _context = context;
    }
    
    public int GetUserTimelineCount(string username)
    {
        return _context.Messages.Count(m => m.author_name == username);
    }
    
    public int GetPublicTimelineCount()
    {
        return _context.Messages.Count();
    }
    
    public IEnumerable<MessageDTO> GetPublicTimeline(int page = 1)
    {
        return _context.Messages
            .OrderByDescending(m => m.pub_date)
            .ThenByDescending(m => m.id)
            .Skip((page - 1) * _messagesPerPage)
            .Take(_messagesPerPage)
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
    }

    public IEnumerable<MessageDTO> GetUserTimeline(int user_id)
    {
        return GetMessages().Where(m => m.author_id == user_id).Take(_messagesPerPage);
    }
    
    public IEnumerable<MessageDTO> GetUserTimeline(string username, int page = 1)
    {
        return _context.Messages
            .Where(m => m.author_name == username)
            .OrderByDescending(m => m.pub_date)
            .Skip((page - 1) * _messagesPerPage)
            .Take(_messagesPerPage)
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
        return _context.Messages
            .OrderByDescending(m => m.pub_date)
            .Select(m => new MessageDTO
            {
                id = m.id,
                author_id = m.author_id,
                author_name = m.author_name,
                author_email = m.author_email,
                text = m.text,
                pub_date = m.pub_date.ToString("yyyy-MM-dd HH:mm:ss")
            });
    }
}