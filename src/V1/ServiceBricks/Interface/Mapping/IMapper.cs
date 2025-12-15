namespace ServiceBricks
{
    public partial interface IMapper
    {        

        TDestination Map<TSource, TDestination>(TSource source) where TDestination : class, new();

        TDestination Map<TSource, TDestination>(TSource source, TDestination destination) where TDestination : class, new();
    }
}