using AutoMapper;

namespace CrispArchitecture.Application.Contracts.v1.Test
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTestDto, Domain.Entities.Test>();
            CreateMap<Domain.Entities.Test, TestResponseDto>();
        }
    }
}