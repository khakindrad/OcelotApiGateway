using Auth.Api.Models.Domain;
using Auth.Api.Models.DTOs;
using FluentResults;

namespace Auth.Api.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AppUser?>> RegisterAsync(RegisterDto registerDto);
        Task<Result<AuthResponse?>> LoginAsync(LoginDto loginDto);
    }
}