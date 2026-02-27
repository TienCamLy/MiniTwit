using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class User : IdentityUser
{
    public required int id { get; set; }
    public required string name { get; set; }
    public required string email { get; set; }
}