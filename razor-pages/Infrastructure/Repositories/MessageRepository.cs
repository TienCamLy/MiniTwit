using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly MiniTwitContext _context;
    private const int _messagesPerPage = 30;
    public FollowerRepository(MiniTwitContext context)
    {
        _context = context;
    }
    
    
}