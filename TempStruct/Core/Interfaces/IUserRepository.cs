using Core.DTOs;

namespace Core.Interfaces;

public interface IFollowerRepository
{
    public UserDTO GetUserByID(int id);
    public UserDTO GetUserByUsername(string username);
    public void CreateUser(string username, string email, string passwordhash);
    public void DeleteUser(int user_id);
}