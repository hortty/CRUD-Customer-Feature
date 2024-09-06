using AutoMapper;
using Customers.Domain.Entities;

namespace Customers.Domain.Dtos
{
    [AutoMap(typeof(Customer), ReverseMap = true)]
    public class UpdateCustomerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PersonType { get; set; }
        public string CpfCnpj { get; set; }
        public bool IsExempt { get; set; } = false;
        public string? StateRegistration { get; set; } = string.Empty;
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsBlocked { get; set; }
    }
}

