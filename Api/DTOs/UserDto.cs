namespace Api.DTOs;

public class UserDto
{
    public required string Username { get; set; }
    public required string? KnownAs { get; set; } = string.Empty;
    public required string Token { get; set; }
    public string? PhotoUrl { get; set; }
}
