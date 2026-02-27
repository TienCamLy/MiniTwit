using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly MiniTwitContext _context;
    public FollowerRepository(MiniTwitContext context)
    {
        _context = context;
    }
    
    
}