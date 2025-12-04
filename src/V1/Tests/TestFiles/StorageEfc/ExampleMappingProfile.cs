using ServiceBricks.Xunit;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit.Mapping
{
    public class ExampleMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ExampleDomain, ExampleDto>(
                (s, d) =>
                {
                    d.CreateDate = s.CreateDate;
                    d.ExampleDate = s.ExampleDate;
                    d.ExampleNullableDate = s.ExampleNullableDate;
                    d.ExampleNullableDateNotSet = s.ExampleNullableDateNotSet;
                    d.Name = s.Name;
                    d.SimpleDate = s.SimpleDate;
                    d.SimpleNullableDate = s.SimpleNullableDate;
                    d.SimpleNullableDateNotSet = s.SimpleNullableDateNotSet;
                    d.StorageKey = s.Key.ToString();
                    d.UpdateDate = s.UpdateDate;
                });

            registry.Register<ExampleDto, ExampleDomain>(
                (s, d) =>
                {
                    d.CreateDate = s.CreateDate;
                    d.ExampleDate = s.ExampleDate;
                    d.ExampleNullableDate = s.ExampleNullableDate;
                    d.ExampleNullableDateNotSet = s.ExampleNullableDateNotSet;
                    int tempStorageKey;
                    if (int.TryParse(s.StorageKey, out tempStorageKey))
                        d.Key = tempStorageKey;
                    d.Name = s.Name;
                    d.SimpleDate = s.SimpleDate;
                    d.SimpleNullableDate = s.SimpleNullableDate;
                    d.SimpleNullableDateNotSet = s.SimpleNullableDateNotSet;
                    d.UpdateDate = s.UpdateDate;
                });
        }
    }

    public class ExampleProcessQueueMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ExampleWorkProcessDomain, ExampleWorkProcessDto>(
                (s, d) =>
                {
                    d.CreateDate = s.CreateDate;
                    d.FutureProcessDate = s.FutureProcessDate;
                    d.IsComplete = s.IsComplete;
                    d.IsError = s.IsError;
                    d.IsProcessing = s.IsProcessing;
                    d.Key = s.Key;
                    d.Name = s.Name;
                    d.ProcessDate = s.ProcessDate;
                    d.ProcessResponse = s.ProcessResponse;
                    d.RetryCount = s.RetryCount;
                    d.StorageKey = s.Key.ToString();
                    d.UpdateDate = s.UpdateDate;
                });

            registry.Register<ExampleWorkProcessDto, ExampleWorkProcessDomain>(
                (s, d) =>
                {
                    d.CreateDate = s.CreateDate;
                    d.FutureProcessDate = s.FutureProcessDate;
                    d.IsComplete = s.IsComplete;
                    d.IsError = s.IsError;
                    d.IsProcessing = s.IsProcessing;
                    d.Key = string.IsNullOrEmpty(s.StorageKey) ? 0 : int.Parse(s.StorageKey);
                    d.Name = s.Name;
                    d.ProcessDate = s.ProcessDate;
                    d.ProcessResponse = s.ProcessResponse;
                    d.RetryCount = s.RetryCount;
                    d.UpdateDate = s.UpdateDate;
                });
        }
    }
}