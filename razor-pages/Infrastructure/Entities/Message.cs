namespace Infrastructure.Entities;

public class Message
{
    public required int message_id { get; set; }
    public required int author_id { get; set; }
    public required string text { get; set; }
    public required DateTime pub_date { get; set; }
    public required string flagged { get; set; }
    public required User author { get; set; }
}