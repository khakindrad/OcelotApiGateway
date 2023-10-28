using Auth.Api.Interfaces;
using Auth.Api.Models.Domain;
using Auth.Api.Models.DTOs;
using Common.Extensions;
using Common.Options;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Services;

public sealed class JwtAuthService : IAuthService
{
    //private readonly List<AppUser> _users = new()
    //{
    //    new("admin", "aDm1n", "Admin", new[] { "articles.read", "articles.delete" }),
    //    new("user01", "u$3r01", "User", new[] { "articles.read" }),
    //    new("user02", "u$3r02", "User", new[] { "writers.read" }),
    //    new("SuperAdmin", "SuperAdmin", "SuperAdmin", new[] { "fullaccess" }),
    //};
    private readonly ILogger<JwtAuthService> _logger;
    private readonly AuthenticationSettings _authSettings;
    private readonly IList<AppUser> _users;

    public JwtAuthService(ILogger<JwtAuthService> logger, IOptions<AuthenticationSettings> authSettings)
    {
        _logger = logger;
        _authSettings = authSettings.Value;
        _users = new List<AppUser>();
    }

    public Task<Result<AuthResponse?>> LoginAsync(LoginDto loginDto)
    {
        var user = _users.FirstOrDefault(u => string.Equals(u.UserName, loginDto.UserName, StringComparison.OrdinalIgnoreCase) && string.Equals(u.Password, loginDto.Password, StringComparison.OrdinalIgnoreCase));

        if (user is null)
        {
            return Task.FromResult<Result<AuthResponse?>>(Result.Fail($"Unable to authenticate user {loginDto.UserName}"));
        }
        else if (_authSettings.JWT is not null)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JWT.Secret));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTimeStamp = DateTime.UtcNow.AddSeconds(_authSettings.JWT.ExpiresInSec);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim("role", user.Role),
            new Claim("scope", string.Join(' ', user.Scopes))
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: _authSettings.JWT.ValidIssuer,
                claims: claims,
                expires: expirationTimeStamp,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Task.FromResult<Result<AuthResponse?>>(new AuthResponse()
            {
                Token = tokenString,
                ExpiresIn = (int)expirationTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            });
        }

        return Task.FromResult<Result<AuthResponse?>>(Result.Fail($"Unable to authenticate user {loginDto.UserName} due to JWT configuration is not found."));
    }

    public Task<Result<AppUser?>> RegisterAsync(RegisterDto registerDto)
    {
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto));

        var userByEmail = _users.FirstOrDefault(x => string.Equals(x.Email, registerDto.Email, StringComparison.OrdinalIgnoreCase));
        var userByUsername = _users.FirstOrDefault(x => string.Equals(x.UserName, registerDto.UserName, StringComparison.OrdinalIgnoreCase));

        if (userByEmail is not null || userByUsername is not null)
        {
            return Task.FromResult<Result<AppUser?>>(Result.Fail(new Error($"User with email {registerDto.Email} or username {registerDto.UserName} already exists.")));
        }

        AppUser appUser = new()
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Password = registerDto.Password,
            Role = "User",
            Scopes = new string[] { "viewonly" }
        };

        _users.Add(appUser);

        _logger.LogMessage($"Added user {registerDto.UserName}");

        return Task.FromResult<Result<AppUser?>>(appUser);
    }
}
