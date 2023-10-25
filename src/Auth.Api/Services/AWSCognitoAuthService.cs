using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Auth.Api.Interfaces;
using Auth.Api.Models.Domain;
using Auth.Api.Models.DTOs;
using Common.Options;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Auth.Api.Services
{
    public class AWSCognitoAuthService : IAuthService
    {
        private readonly ILogger<AWSCognitoAuthService> _logger;
        private readonly AuthenticationSettings _authSettings;
        private readonly RegionEndpoint _region = RegionEndpoint.APSouth1;
        private readonly AmazonCognitoIdentityProviderClient _cognito;

        public AWSCognitoAuthService(ILogger<AWSCognitoAuthService> logger, IOptions<AuthenticationSettings> authSettings)
        {
            _logger = logger;
            _authSettings = authSettings.Value;

            _region = RegionEndpoint.GetBySystemName(_authSettings.AWSCognito.Region);
            _cognito = new AmazonCognitoIdentityProviderClient(_region);
        }

        public async Task<Result<AuthResponse?>> LoginAsync(LoginDto loginDto)
        {
            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = _authSettings.AWSCognito.UserPoolId,
                ClientId = _authSettings.AWSCognito.ClientId,
                AuthFlow = new AuthFlowType(_authSettings.AWSCognito.AuthFlowType)
            };

            request.AuthParameters.Add("USERNAME", loginDto.UserName);
            request.AuthParameters.Add("PASSWORD", loginDto.Password);

            var response = await _cognito.AdminInitiateAuthAsync(request);

            AuthResponse authResponse = new()
            {
                Token = response.AuthenticationResult.IdToken,
                ExpiresIn = response.AuthenticationResult.ExpiresIn,
            };

            return authResponse;
        }

        public async Task<Result<AppUser?>> RegisterAsync(RegisterDto registerDto)
        {
            var request = new SignUpRequest
            {
                ClientId = _authSettings.AWSCognito.ClientId,
                Password = registerDto.Password,
                Username = registerDto.UserName
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = registerDto.Email
            };
            request.UserAttributes.Add(emailAttribute);

            var response = await _cognito.SignUpAsync(request);

            _logger.LogInformation($"Added user {registerDto.UserName} {response}");

            AppUser appUser = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            return appUser;
        }
    }
}