using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<ActorCreateDTO, Actor>();
            CreateMap<ActorUpdateDTO, Actor>();
            CreateMap<Actor, ActorDTO>();
            CreateMap<Actor, ActorBasicDTO>();
            CreateMap<Actor, ActorDTO>();
        }
    }
}
