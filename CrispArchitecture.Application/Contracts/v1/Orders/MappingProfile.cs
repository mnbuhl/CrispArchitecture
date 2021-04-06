using AutoMapper;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Contracts.v1.Orders
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderCommandDto, Order>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<LineItemCommandDto, LineItem>();
            CreateMap<LineItem, LineItemResponseDto>();
        }
    }
}