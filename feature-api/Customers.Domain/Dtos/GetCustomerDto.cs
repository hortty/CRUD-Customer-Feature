using AutoMapper;
using Customers.Domain.Entities;

namespace Customers.Domain.Dtos
{
    [AutoMap(typeof(Customer), ReverseMap = true)]
    public class GetCustomerDto : GetPagedBaseDto
    {
        public long? Id { get; set; }

        public string? Name { get; set; } = string.Empty;
    }
}
