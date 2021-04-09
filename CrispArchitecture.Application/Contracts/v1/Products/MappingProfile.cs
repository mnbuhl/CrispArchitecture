using AutoMapper;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Contracts.v1.Products
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCommandDto, Product>();
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.ProductGroup, opt =>
                    opt.MapFrom(src => src.ProductGroup.Name));
        }
    }
}