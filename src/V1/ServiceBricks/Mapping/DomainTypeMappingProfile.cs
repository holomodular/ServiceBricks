namespace ServiceBricks
{
    /// <summary>
    /// A mapping profile for domaintype.
    /// </summary>
    public partial class DomainTypeMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<DomainType, DomainTypeDto>(
                (s, d) =>
                {
                    d.Name = s.Name;
                    d.StorageKey = s.Key;
                });

            registry.Register<DomainTypeDto, DomainType>(
                (s, d) =>
                {
                    d.Name = s.Name;
                    d.Key = s.StorageKey;
                });
        }
    }
}