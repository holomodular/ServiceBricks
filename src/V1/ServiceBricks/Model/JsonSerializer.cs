namespace ServiceBricks
{
    public partial class JsonSerializer : IJsonSerializer
    {
        public static IJsonSerializer Instance = new JsonSerializer();

        public T DeserializeObject<T>(string data)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(data, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        public object DeserializeObject(string data, Type objectType)
        {
            return System.Text.Json.JsonSerializer.Deserialize(data, objectType, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        public string SerializeObject(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
}