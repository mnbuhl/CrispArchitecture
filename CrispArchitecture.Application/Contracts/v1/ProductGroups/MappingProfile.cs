using AutoMapper;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Contracts.v1.ProductGroups
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductGroupCommandDto, ProductGroup>();
            CreateMap<ProductGroup, ProductGroupResponseDto>();
            CreateMap<ProductGroup, ProductGroupWithProductsResponseDto>();
        }
    }
}