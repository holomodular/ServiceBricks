using AutoMapper;

namespace ServiceBricks
{
    public class DomainTypeMappingProfile : Profile
    {
        public DomainTypeMappingProfile()
        {
            CreateMap<DomainType, DomainTypeDto>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ReverseMap();
        }
    }
}