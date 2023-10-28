namespace Auth.Api.Models.DTOs;

public sealed class AuthResponseDto
{
    public required string Token { get; set; }
    public required int ExpiresIn { get; set; }
}
