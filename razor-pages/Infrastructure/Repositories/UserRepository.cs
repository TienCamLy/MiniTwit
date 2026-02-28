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
        var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDTO
        {
            id = user.Id,
            name = user.UserName,
            email = user.Email,
        };
    }

    public UserDTO GetUserByUsername(string username)
    {
        var user = _context.Users.Where(u => u.UserName == username).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDTO
        {
            id = user.Id,
            name = user.UserName,
            email = user.Email,
        };
    }

    public UserDTO Login(string username, string password)
    {
        var user = _context.Users.Where(u => u.UserName == username).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var hash = new PasswordHasher<User>();
        var verifyHashResult = hash.VerifyHashedPassword(user, user.PasswordHash, password);
        if (verifyHashResult == PasswordVerificationResult.Failed)
        {
            throw new Exception("Invalid password");
        }
        
        return new UserDTO
        {
            id = user.Id,
            name = user.UserName,
            email = user.Email,
        };
    }

    public void CreateUser(string username, string email, string password)
    {
        var existingUser = _context.Users.Where(u => u.UserName == username).FirstOrDefault();

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

    public void DeleteUser(int user_id)
    {
        var existingUser = _context.Users.Where(u => u.Id == user_id).FirstOrDefault();

        if (existingUser is null)
        {
            throw new ArgumentException("User not found");
        }
        
        // TODO: Remove all messages and followers
        
        _context.Users.Remove(existingUser);
        _context.SaveChanges();
    }
}