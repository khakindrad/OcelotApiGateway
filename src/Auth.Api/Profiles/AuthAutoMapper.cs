using Auth.Api.Models.Domain;
using Auth.Api.Models.DTOs;
using AutoMapper;

namespace Auth.Api.Profiles;

public sealed class AuthAutoMapper : Profile
{
    public AuthAutoMapper()
    {
        CreateMap<AppUser, RegisterResponseDto>().ReverseMap();
        CreateMap<AuthResponse, AuthResponseDto>().ReverseMap();
    }
}
