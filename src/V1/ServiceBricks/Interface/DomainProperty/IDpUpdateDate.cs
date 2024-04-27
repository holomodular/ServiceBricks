namespace ServiceBricks
{
    /// <summary>
    /// This property is required for an object.
    /// </summary>
    public interface IDpUpdateDate
    {
        DateTimeOffset UpdateDate { get; set; }
    }
}