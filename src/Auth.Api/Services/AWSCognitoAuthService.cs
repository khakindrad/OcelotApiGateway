using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Auth.Api.Interfaces;
using Auth.Api.Models.Domain;
using Auth.Api.Models.DTOs;
using Common.Extensions;
using Common.Options;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Auth.Api.Services;

public sealed class AwsCognitoAuthService : IAuthService, IDisposable
{
    private readonly ILogger<AwsCognitoAuthService> _logger;
    private readonly AwsCognitoAuthSettings? _authSettings;
    private readonly AmazonCognitoIdentityProviderClient? _cognito;

    public AwsCognitoAuthService(ILogger<AwsCognitoAuthService> logger, IOptions<AuthenticationSettings> authSettings)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(authSettings));
        ArgumentException.ThrowIfNullOrEmpty(nameof(authSettings.Value));
        ArgumentException.ThrowIfNullOrEmpty(nameof(authSettings.Value.AWSCognito));

        _logger = logger;
        _authSettings = authSettings.Value.AWSCognito;

        if (_authSettings is not null)
        {
            var region = RegionEndpoint.GetBySystemName(_authSettings.Region);
            _cognito = new AmazonCognitoIdentityProviderClient(region);
        }
    }

    public async Task<Result<AuthResponse?>> LoginAsync(LoginDto loginDto)
    {
        if (_authSettings is null)
        {
            return Result.Fail("_authSettings is null");
        }

        if (_cognito is null)
        {
            return Result.Fail("_cognito is null");
        }

        var request = new AdminInitiateAuthRequest
        {
            UserPoolId = _authSettings.UserPoolId,
            ClientId = _authSettings.ClientId,
            AuthFlow = new AuthFlowType(_authSettings.AuthFlowType)
        };

        request.AuthParameters.Add("USERNAME", loginDto.UserName);
        request.AuthParameters.Add("PASSWORD", loginDto.Password);

        var response = await _cognito.AdminInitiateAuthAsync(request).ConfigureAwait(false);

        return new AuthResponse()
        {
            Token = response.AuthenticationResult.IdToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn,
        };
    }

    public async Task<Result<AppUser?>> RegisterAsync(RegisterDto registerDto)
    {
        if (_authSettings is null)
        {
            return Result.Fail("_authSettings is null");
        }

        if (_cognito is null)
        {
            return Result.Fail("_cognito is null");
        }

        var request = new SignUpRequest
        {
            ClientId = _authSettings.ClientId,
            Password = registerDto.Password,
            Username = registerDto.UserName
        };

        var emailAttribute = new AttributeType
        {
            Name = "email",
            Value = registerDto.Email
        };
        request.UserAttributes.Add(emailAttribute);

        var response = await _cognito.SignUpAsync(request).ConfigureAwait(false);

        _logger.LogMessage(LogLevel.Debug, $"Added user {registerDto.UserName} {response}");

        return new AppUser()
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Password = registerDto.Password,
            Role = "User",
            Scopes = new string[] { "viewonly" }
        };
    }

    public void Dispose()
    {
        _cognito?.Dispose();
    }
}
