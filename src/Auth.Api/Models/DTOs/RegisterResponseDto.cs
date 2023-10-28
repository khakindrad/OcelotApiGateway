namespace Auth.Api.Models.DTOs;

public sealed class RegisterResponseDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}
