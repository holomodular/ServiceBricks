namespace ServiceBricks
{
    public class Paging : IPaging
    {
        public virtual int PageNumber { get; set; }
        public virtual int PageSize { get; set; }
    }
}