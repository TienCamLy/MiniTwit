namespace Core.DTOs;

public class MessageDTO
{
    public required int Id { get; init; }
    public required string Text { get; init; }
    public required string PubDate { get; init; }

    // Extra fields added for the frontend
	public required string AuthorName { get; init; }
	public required string AuthorEmail { get; init; }
}