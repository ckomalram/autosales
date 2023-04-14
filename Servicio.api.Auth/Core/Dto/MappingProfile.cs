using AutoMapper;
using Servicio.api.Auth.Core.Entities;

namespace Servicio.api.Auth.Core.Dto;


public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<User, UserDto>();

        //.. si se desea mappear otra entidad, puedes colocarla aqui....

    }

}