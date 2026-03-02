using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class User : IdentityUser<int>
{
    public IEnumerable<Message> Messages { get; set; }
}