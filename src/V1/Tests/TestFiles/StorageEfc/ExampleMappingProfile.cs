using AutoMapper;
using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Mapping
{
    public class ExampleMappingProfile : Profile
    {
        public ExampleMappingProfile()
        {
            CreateMap<ExampleDto, ExampleDomain>()
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>());

            CreateMap<ExampleDomain, ExampleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key));
        }

        public class KeyResolver : IValueResolver<DataTransferObject, object, int>
        {
            public int Resolve(DataTransferObject source, object destination, int sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.StorageKey))
                    return 0;

                int tempKey;
                if (int.TryParse(source.StorageKey, out tempKey))
                    return tempKey;
                return 0;
            }
        }
    }
}