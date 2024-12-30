using Api.DTOs;
using Api.Entities;
using AutoMapper;

namespace Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
        .ForMember(d => d.PhotoUrl, o => o.MapFrom(s=>s.Photos.FirstOrDefault(x=>x.IsMain)!.Url));
        CreateMap<Photo, PhotoDto>();
        CreateMap<UserUpdateDto, AppUser>();
    }
}
