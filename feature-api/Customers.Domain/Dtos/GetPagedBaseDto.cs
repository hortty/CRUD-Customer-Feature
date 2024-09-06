namespace Customers.Domain.Dtos
{
    public class GetPagedBaseDto
    {
        public int PageSize { get; set; } = 20;
        public int Page { get; set; } = 1;
    }
}