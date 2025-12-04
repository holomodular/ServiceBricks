using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ServiceBricks
{
    public class Mapper : IMapper
    {
        public static IMapper Instance = new Mapper(MapperRegistry.Instance);

        private readonly IMapperRegistry _mapperRegistry;

        public Mapper(IMapperRegistry mapperRegistry)
        {
            _mapperRegistry = mapperRegistry;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class, new()
        {
            if (source == null)
                return null;

            var destination = new TDestination();
            return Map(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
            where TDestination : class, new()
        {
            if (source == null || destination == null)
                return null;

            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            // If both source and destination are collections, map element-by-element
            if (TryGetEnumerableElementType(sourceType, out var sourceElementType) &&
                TryGetEnumerableElementType(destinationType, out var destElementType))
            {
                MapCollection(source, destination, sourceElementType, destElementType);
                return destination;
            }

            // Single-object mapping (existing behavior)
            var mapper = _mapperRegistry.GetMapper(sourceType, destinationType);
            if (mapper == null)
                throw new BusinessException($"Missing mapping for {sourceType} to {destinationType}");

            mapper(source, destination);
            return destination;
        }

        public TDestination Map<TDestination>(object source)
            where TDestination : class, new()
        {
            if (source == null)
                return null;

            var sourceType = source.GetType();
            var destinationType = typeof(TDestination);

            // If both source and destination are collections, map element-by-element
            if (TryGetEnumerableElementType(sourceType, out var sourceElementType) &&
                TryGetEnumerableElementType(destinationType, out var destElementType))
            {
                var destination = new TDestination();
                MapCollection(source, destination, sourceElementType, destElementType);
                return destination;
            }

            // Single-object mapping (existing behavior)
            var mapper = _mapperRegistry.GetMapper(sourceType, destinationType);
            if (mapper == null)
                throw new BusinessException($"Missing mapping for {sourceType} to {destinationType}");

            var dest = new TDestination();
            mapper(source, dest);
            return dest;
        }

        /// <summary>
        /// Map a collection of source elements into a collection of destination elements.
        /// Relies on a registered mapper for element types (sourceElementType → destElementType).
        /// </summary>
        private void MapCollection(object sourceCollection,
                                   object destinationCollection,
                                   Type sourceElementType,
                                   Type destElementType)
        {
            var itemMapper = _mapperRegistry.GetMapper(sourceElementType, destElementType);
            if (itemMapper == null)
                throw new BusinessException($"Missing mapping for {sourceElementType} to {destElementType}");

            if (sourceCollection is not IEnumerable sourceEnumerable)
                throw new BusinessException($"Source type {sourceCollection.GetType()} is not enumerable.");

            if (destinationCollection is not IList destList)
                throw new BusinessException(
                    $"Destination type {destinationCollection.GetType()} must implement IList to map collections.");

            foreach (var srcItem in sourceEnumerable)
            {
                var destItem = Activator.CreateInstance(destElementType);
                itemMapper(srcItem, destItem);
                destList.Add(destItem);
            }
        }

        /// <summary>
        /// Try to get the element type of an enumerable (e.g., List&lt;T&gt;, IEnumerable&lt;T&gt;, T[]).
        /// Returns false for string to avoid treating it as IEnumerable&lt;char&gt;.
        /// </summary>
        private static bool TryGetEnumerableElementType(Type type, out Type elementType)
        {
            elementType = null;

            if (type == typeof(string))
                return false;

            // Arrays
            if (type.IsArray)
            {
                elementType = type.GetElementType();
                return true;
            }

            // Generic types: List<T>, IEnumerable<T>, ICollection<T>, etc.
            if (type.IsGenericType)
            {
                var genDef = type.GetGenericTypeDefinition();
                if (genDef == typeof(List<>) ||
                    genDef == typeof(IList<>) ||
                    genDef == typeof(ICollection<>) ||
                    genDef == typeof(IEnumerable<>))
                {
                    elementType = type.GetGenericArguments()[0];
                    return true;
                }
            }

            // Check implemented interfaces for IEnumerable<T>
            var enumInterface = type
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType &&
                                     i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumInterface != null)
            {
                elementType = enumInterface.GetGenericArguments()[0];
                return true;
            }

            return false;
        }
    }
}