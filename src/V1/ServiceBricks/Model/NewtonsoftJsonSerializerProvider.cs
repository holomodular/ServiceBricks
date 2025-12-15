using Newtonsoft.Json;

namespace ServiceBricks
{
    public partial class NewtonsoftJsonSerializerProvider : IJsonSerializer
    {
        public T DeserializeObject<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public object DeserializeObject(string data, Type objectType)
        {
            return JsonConvert.DeserializeObject(data, objectType);
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}