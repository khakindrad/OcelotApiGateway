namespace Auth.Api.Models
{
    public record User(string Username, string Password, string Role, string[] Scopes);
}
