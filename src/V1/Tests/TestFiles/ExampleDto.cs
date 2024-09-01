namespace ServiceBricks.Xunit
{
    public class ExampleDto : DataTransferObject
    {
        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset ExampleDate { get; set; }
    }

    public class ExampleDomain : IDomainObject<ExampleDomain>, IDpCreateDate, IDpUpdateDate
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset ExampleDate { get; set; }
    }
}