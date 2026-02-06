namespace razor_pages.Structs;

public class Message
{
    public required int message_id { get; set; }
    public required int author_id { get; set; }
    public required string text { get; set; }
    public required string pub_date { get; set; }
    
    public required string flagged { get; set; }
}