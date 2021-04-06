using AutoMapper;

namespace CrispArchitecture.Application.Contracts.v1.TestOwner
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTestOwnerDto, Domain.Entities.TestOwner>();
            CreateMap<UpdateTestOwnerDto, Domain.Entities.TestOwner>();
            CreateMap<Domain.Entities.TestOwner, TestOwnerResponseDto>();
        }
    }
}