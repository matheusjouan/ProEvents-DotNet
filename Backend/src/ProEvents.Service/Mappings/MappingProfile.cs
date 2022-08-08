using AutoMapper;
using ProEvents.Domain.Identity;
using ProEvents.Domain.Model;
using ProEvents.Service.DTOs;

namespace ProEvents.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<Speaker, SpeakerDTO>().ReverseMap();
            CreateMap<Speaker, SpeakerAddDTO>().ReverseMap();
            CreateMap<Speaker, SpeakerUpdateDTO>().ReverseMap();
            CreateMap<SocialNetwork, SocialNetworkDTO>().ReverseMap();
            CreateMap<Batch, BatchDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
        }
    }
}