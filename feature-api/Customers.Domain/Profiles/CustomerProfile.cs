using AutoMapper;
using Customers.Domain.Dtos;
using Customers.Domain.Entities;
using Customers.Domain.Enums;

namespace Customers.Domain.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(
                dest => dest.PersonType, opt => 
            opt.MapFrom(src => Enum.Parse<PersonType>(src.PersonType))
            ).ForMember(dest => dest.Gender, opt => 
            opt.MapFrom(src => src.Gender != null ? Enum.Parse<Gender>(src.Gender) : (Gender?)null));

        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.PersonType, opt => 
            opt.MapFrom(src => Enum.Parse<PersonType>(src.PersonType)))
            .ForMember(dest => dest.Gender, opt => 
            opt.MapFrom(src => src.Gender != null ? Enum.Parse<Gender>(src.Gender) : (Gender?)null));

        CreateMap<Customer, FoundCustomerDto>()
            .ForMember(dest => dest.PersonType, opt => 
            opt.MapFrom(src => src.PersonType.ToString()))
            .ForMember(dest => dest.Gender, opt => 
            opt.MapFrom(src => src.Gender.HasValue ? src.Gender.ToString() : null));
    }
}
