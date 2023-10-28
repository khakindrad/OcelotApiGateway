namespace Auth.Api.Models.Domain;

public sealed class AuthResponse
{
    public required string Token { get; set; }
    public required int ExpiresIn { get; set; }
}
