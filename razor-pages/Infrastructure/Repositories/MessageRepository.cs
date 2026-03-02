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

    public IEnumerable<MessageDTO> GetPublicTimeline()
    {
        return _context.Messages
            .OrderByDescending(m => m.PubDate)
            .Select(m => new MessageDTO
            {
                Id = m.Id,
                Text = m.Text,
                PubDate = m.PubDate.ToString("yyyy-MM-dd HH:mm:ss"),
				AuthorName = m.Author.UserName,
                AuthorEmail = m.Author.Email
            })
			.ToList();
    }

    public IEnumerable<MessageDTO> GetPublicTimelinePage(int page = 1)
    {
        return _context.Messages
            .OrderByDescending(m => m.PubDate)
			.Skip((page - 1) * _messagesPerPage)
			.Take(_messagesPerPage)
            .Select(m => new MessageDTO
            {
                Id = m.Id,
                Text = m.Text,
                PubDate = m.PubDate.ToString("yyyy-MM-dd HH:mm:ss"),
				AuthorName = m.Author.UserName,
                AuthorEmail = m.Author.Email
            })
			.ToList();
    }

    public IEnumerable<MessageDTO> GetUserTimeline(int user_id)
    {
        return _context.Messages
            .OrderByDescending(m => m.PubDate)
			.Where(m => m.Author.Id == user_id)
            .Select(m => new MessageDTO
            {
                Id = m.Id,
                Text = m.Text,
                PubDate = m.PubDate.ToString("yyyy-MM-dd HH:mm:ss"),
				AuthorName = m.Author.UserName,
                AuthorEmail = m.Author.Email
            })
			.ToList();
    }
    
    public IEnumerable<MessageDTO> GetUserTimelinePage(string username, int page = 1)
    {
		return _context.Messages
            .OrderByDescending(m => m.PubDate)
			.Where(m => m.Author.UserName == username)
			.Skip((page - 1) * _messagesPerPage)
			.Take(_messagesPerPage)
            .Select(m => new MessageDTO
            {
                Id = m.Id,
                Text = m.Text,
                PubDate = m.PubDate.ToString("yyyy-MM-dd HH:mm:ss"),
				AuthorName = m.Author.UserName,
                AuthorEmail = m.Author.Email
            })
			.ToList();
    }
    
	public int GetUserTimelineCount(string username)
    {
        return _context.Messages.Count(m => m.Author.UserName == username);
    }
    
    public int GetPublicTimelineCount()
    {
        return _context.Messages.Count();
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
            AuthorId = author_id,
            Author = user,
            Text = text,
            PubDate = DateTime.UtcNow,
            Flagged = 0
        };
        
        _context.Messages.Add(message);
        _context.SaveChanges();
    }
}