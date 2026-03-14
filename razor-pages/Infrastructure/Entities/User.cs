using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class User : IdentityUser<int>
{
    // Email, UserName, Id, PasswordHash (implicit)
    public IEnumerable<Message> Messages { get; set; }
}