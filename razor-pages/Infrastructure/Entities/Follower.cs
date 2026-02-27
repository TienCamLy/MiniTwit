namespace Infrastructure.Entities;

public class Follower
{
    public required User source { get; set; } // Follower
    public required User target { get; set; } // Followee
}