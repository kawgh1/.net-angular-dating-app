using System.Linq;
using API.DTO;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // Here we are mapping the first photo that has isMain == true to be the MemberDTO's PhotoUrl photo
        CreateMap<AppUser, MemberDTO>()

            .ForMember(member => member.PhotoUrl,
                options => options.MapFrom(
                    src => src.Photos.FirstOrDefault(photo => photo.IsMain).Url))

            .ForMember(member => member.Age,
                options => options.MapFrom(
                    src => src.DateOfBirth.CalculateAge()));
        
        CreateMap<Photo, PhotoDTO>();

        CreateMap<MemberUpdateDTO, AppUser>();
    }
}