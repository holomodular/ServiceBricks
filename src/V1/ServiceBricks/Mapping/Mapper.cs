using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        /// <summary>
        /// Map
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Map<TSource, TDestination>(TSource source)
              where TDestination : class, new()
        {
            if (source == null)
                return null;

            var destination = new TDestination();
            return Map(source, destination);
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
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

            // Single-object mapping
            var mapper = _mapperRegistry.GetMapper(sourceType, destinationType);
            if (mapper == null)
            {
                // Fallback: same type, use public properties
                if (sourceType == destinationType)
                {
                    MapByPublicProperties(source, destination, sourceType);
                    return destination;
                }

                throw new BusinessException($"Missing mapping for {sourceType} to {destinationType}");
            }

            mapper(source, destination);
            return destination;
        }

        /// <summary>
        /// Fallback method to map public properties
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="type"></param>
        private void MapByPublicProperties(object source, object destination, Type type)
        {
            if (source == null || destination == null)
                return;

            var props = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in props)
            {
                var value = prop.GetValue(source);
                prop.SetValue(destination, value);
            }
        }

        private void MapCollection(
     object sourceCollection,
     object destinationCollection,
     Type sourceElementType,
     Type destElementType)
        {
            var itemMapper = _mapperRegistry.GetMapper(sourceElementType, destElementType);

            if (sourceCollection is not IEnumerable sourceEnumerable)
                throw new BusinessException($"Source type {sourceCollection.GetType()} is not enumerable.");

            if (destinationCollection is not IList destList)
                throw new BusinessException($"Destination type {destinationCollection.GetType()} must implement IList to map collections.");

            if (itemMapper == null && sourceElementType != destElementType)
                throw new BusinessException($"Missing mapping for {sourceElementType} to {destElementType}");

            foreach (var srcItem in sourceEnumerable)
            {
                // Create an element instance, not the collection type.
                var destItem = Activator.CreateInstance(destElementType);
                if (destItem == null)
                    throw new BusinessException($"Unable to create instance of {destElementType}.");

                if (itemMapper != null)
                {
                    itemMapper(srcItem, destItem);
                }
                else
                {
                    // Fallback when element types are the same
                    MapByPublicProperties(srcItem, destItem, destElementType);
                }

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