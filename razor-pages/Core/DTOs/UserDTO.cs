namespace Core.DTOs;

public class UserDTO
{
    public required int Id { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}