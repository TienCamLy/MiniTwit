using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly MiniTwitContext _context;
    public UserRepository(MiniTwitContext context)
    {
        _context = context;
    }
    
    
}