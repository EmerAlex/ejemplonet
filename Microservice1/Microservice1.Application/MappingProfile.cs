using AutoMapper;

namespace Microservice1.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.Person, Microservice1.Application.Person.PersonDto>();
            CreateMap<Microservice1.Application.Person.PersonDto, Domain.Entities.Person>();
            CreateMap<Domain.Entities.Address, Microservice1.Application.Person.AddressDto>();
            CreateMap<Microservice1.Application.Person.AddressDto, Domain.Entities.Address>();
        }
    }
}
