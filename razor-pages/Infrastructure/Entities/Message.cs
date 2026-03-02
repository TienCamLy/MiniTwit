using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }
	
	public required User Author { get; set; }
    public required int AuthorId { get; set; }
	
    public required string Text { get; set; }
    public required DateTime PubDate { get; set; }

    public required int Flagged { get; set; }
}