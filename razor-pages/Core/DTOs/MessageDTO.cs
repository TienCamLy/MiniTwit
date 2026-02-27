namespace Core.DTOs;

public class MessageDTO
{
    public required int id { get; init; }
    public required int author_id { get; init; }
    public required string text { get; init; }
    public required string pub_date { get; init; }
}