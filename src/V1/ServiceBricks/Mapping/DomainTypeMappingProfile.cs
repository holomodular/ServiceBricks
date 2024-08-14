using AutoMapper;

namespace ServiceBricks
{
    /// <summary>
    /// A mapping profile for domain types.
    /// </summary>
    public partial class DomainTypeMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public DomainTypeMappingProfile()
        {
            CreateMap<DomainType, DomainTypeDto>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ReverseMap();
        }
    }
}