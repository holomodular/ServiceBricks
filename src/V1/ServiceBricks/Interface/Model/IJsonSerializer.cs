namespace ServiceBricks
{
    public partial interface IJsonSerializer
    {
        string SerializeObject(object obj);

        T DeserializeObject<T>(string data);

        object DeserializeObject(string data, Type objectType);
    }
}