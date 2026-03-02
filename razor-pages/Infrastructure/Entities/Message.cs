using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Entities;

public class Message
{
    [Key]
    public int id { get; set; }
    public required int author_id { get; set; }
    public required string author_name { get; set; }
    public required string author_email { get; set; }
    public required string text { get; set; }
    public required DateTime pub_date { get; set; }

    public required string flagged { get; set; }
}