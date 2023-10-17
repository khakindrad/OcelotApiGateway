namespace Auth.Api.Models
{
    public record AuthenticationToken(string Token, int ExpiresIn);
}
