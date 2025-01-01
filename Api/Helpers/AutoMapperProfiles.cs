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
        CreateMap<RegisterDTO, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s=>DateOnly.Parse(s));//Method to parse string fields to DateOnly fields using automapper as from RegisterDTO we are passing excepting date as string and mapping in AppUser Dateonly field
    }
}
