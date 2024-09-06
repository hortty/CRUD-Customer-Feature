namespace Customers.Domain.Dtos
{
    public class PagedEnvelopDto<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Pages { get; set; }
    }
}