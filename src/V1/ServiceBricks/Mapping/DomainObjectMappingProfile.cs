using AutoMapper;

namespace ServiceBricks
{
    /// <summary>
    /// A base class for mapping domain objects to DTO objects and vice versa.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TDtoObject"></typeparam>
    public abstract partial class DomainObjectMappingProfile<TDomainObject, TDtoObject> : Profile where TDomainObject : IDomainObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public DomainObjectMappingProfile()
        {
            CreateMap<TDomainObject, TDtoObject>().ReverseMap();
        }
    }
}