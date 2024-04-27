namespace ServiceBricks
{
    public class ApplicationOptions
    {
        public ApplicationOptions()
        {
            Name = "Service Bricks";
            Url = "https://localhost:7000";
        }

        public string Name { get; set; }
        public string Url { get; set; }
    }
}