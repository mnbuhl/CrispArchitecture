using AutoMapper;
using CrispArchitecture.Application.Contracts.v1.Test;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Contracts.v1.Customers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerCommandDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>();
        }
    }
}