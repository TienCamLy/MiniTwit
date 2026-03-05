using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly MiniTwitContext _context;
    public UserRepository(MiniTwitContext context)
    {
        _context = context;
    }

    public UserDTO GetUserByID(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        };
    }

    public UserDTO GetUserByUsername(string username)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == username);
        if (user == null)
        {
            return null;
        }

        return new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        };
    }

    public UserDTO Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == username);
        if (user == null)
        {
            return null;
        }
        
        var hash = new PasswordHasher<User>();
        var verifyHashResult = hash.VerifyHashedPassword(user, user.PasswordHash, password);
        if (verifyHashResult == PasswordVerificationResult.Failed)
        {
            return null;
        }
        
        return new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        };
    }

    public void CreateUser(string username, string email, string password)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.UserName == username);

        if (existingUser is not null)
        {
            throw new ArgumentException("User already exists: ", username);
        }
        
        var hash = new PasswordHasher<User>(); 
        
        var user = new User
        {
            UserName = username,
            Email = email,
            PasswordHash = hash.HashPassword(null, password),
        };
            
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void DeleteUser(int userId)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (existingUser is null)
        {
            throw new ArgumentException("User not found");
        }
        
		var userMessages = _context.Messages.Where(m => m.Author == existingUser).ToList();
		var followers = _context.Followers.Where(f => f.SourceId == userId || f.TargetId == userId).ToList();
        
		_context.Followers.RemoveRange(followers);
		_context.Messages.RemoveRange(userMessages);
        _context.Users.Remove(existingUser);

        _context.SaveChanges();
    }
}