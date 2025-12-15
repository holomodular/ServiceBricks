namespace ServiceBricks
{
    public partial class JsonSerializer : IJsonSerializer
    {
        public static IJsonSerializer Instance = new NewtonsoftJsonSerializerProvider();

        public T DeserializeObject<T>(string data)
        {
            return Instance.DeserializeObject<T>(data);
        }

        public object DeserializeObject(string data, Type objectType)
        {
            return Instance.DeserializeObject(data, objectType);
        }

        public string SerializeObject(object obj)
        {
            return Instance.SerializeObject(obj);
        }
    }
}