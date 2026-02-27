using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Entities;
using Infrastructure.Context;
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
        throw new NotImplementedException();
    }

    public UserDTO GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public UserDTO Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public void CreateUser(string username, string email, string passwordhash)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(int user_id)
    {
        throw new NotImplementedException();
    }
}