using AutoMapper;
using CrispArchitecture.Domain.Entities.Identity;

namespace CrispArchitecture.Application.Contracts.v1.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Address, AddressResponseDto>();
        }
    }
}