namespace Auth.Api.Models.Domain;

public sealed class AppUser
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string[] Scopes { get; set; }
}
