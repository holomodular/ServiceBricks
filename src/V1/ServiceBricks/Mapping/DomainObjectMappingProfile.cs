using AutoMapper;

namespace ServiceBricks
{
    public abstract class DomainObjectMappingProfile<TDomainObject, TDtoObject> : Profile where TDomainObject : IDomainObject
    {
        public DomainObjectMappingProfile()
        {
            CreateMap<TDomainObject, TDtoObject>().ReverseMap();
        }
    }
}