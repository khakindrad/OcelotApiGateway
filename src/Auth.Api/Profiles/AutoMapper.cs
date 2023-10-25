using Auth.Api.Models.Domain;
using Auth.Api.Models.DTOs;
using AutoMapper;

namespace Auth.Api.Profiles
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<AppUser, RegisterResponseDto>().ReverseMap();
            CreateMap<AuthResponse, AuthResponseDto>().ReverseMap();
        }
    }
}
