using Auth.Api.Models;

namespace Auth.Api.Services
{
    public interface IJwtTokenService
    {
        AuthenticationToken? GenerateAuthToken(LoginModel loginModel);
    }
}