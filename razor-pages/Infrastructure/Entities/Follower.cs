namespace Infrastructure.Entities;

public class Follower
{
    public required User Source { get; set; } // Follower
    public int SourceId { get; set; } // Follower id

    public required User Target { get; set; } // Followee
    public int TargetId { get; set; } // Followee id
}