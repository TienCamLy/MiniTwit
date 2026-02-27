namespace Infrastructure.Entities;

public class Follower
{
    public required User source { get; set; } // Follower
    public int source_id { get; set; } // Follower id
    public required User target { get; set; } // Followee
    public int target_id { get; set; } // Followee id
}