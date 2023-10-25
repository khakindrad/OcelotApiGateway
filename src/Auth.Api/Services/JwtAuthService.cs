using Auth.Api.Interfaces;
using Auth.Api.Models.Domain;
using Auth.Api.Models.DTOs;
using Common.Options;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Services
{
    public class JwtAuthService : IAuthService
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

        public async Task<Result<AuthResponse?>> LoginAsync(LoginDto loginDto)
        {
            var user = _users.FirstOrDefault(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            if (user is null)
            {
                return Result.Fail($"Unable to authenticate user {loginDto.UserName}");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JWT.Secret));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTimeStamp = DateTime.Now.AddSeconds(_authSettings.JWT.ExpiresInSec);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("role", user.Role),
                new Claim("scope", string.Join(" ", user.Scopes))
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: _authSettings.JWT.ValidIssuer,
                claims: claims,
                expires: expirationTimeStamp,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            AuthResponse authResponse = new()
            {
                Token = tokenString,
                ExpiresIn = (int)expirationTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };

            return authResponse;
        }

        public async Task<Result<AppUser?>> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
                throw new ArgumentNullException(nameof(registerDto));

            var userByEmail = _users.FirstOrDefault(x => x.Email == registerDto.Email);
            var userByUsername = _users.FirstOrDefault(x => x.UserName == registerDto.UserName);

            if (userByEmail is not null || userByUsername is not null)
            {
                return Result.Fail(new Error($"User with email {registerDto.Email} or username {registerDto.UserName} already exists."));
            }

            AppUser appUser = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password,
            };

            _users.Add(appUser);

            _logger.LogInformation($"Added user {registerDto.UserName}");

            return appUser;
        }
    }
}