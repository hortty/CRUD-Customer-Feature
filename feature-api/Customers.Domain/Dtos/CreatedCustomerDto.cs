using AutoMapper;
using Customers.Domain.Entities;

namespace Customers.Domain.Dtos
{
    [AutoMap(typeof(Customer), ReverseMap = true)]
    public class CreatedCustomerDto
    {
        public long Id { get; set; }

        public string Name { get; set; } = String.Empty;
    }
}