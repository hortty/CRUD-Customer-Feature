using AutoMapper;
using Customers.Domain.Entities;

namespace Customers.Domain.Dtos
{
    [AutoMap(typeof(Customer), ReverseMap = true)]
    public class FoundCustomerDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public bool IsBlocked { get; set; }
        public string PersonType { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string StateRegistration { get; set; } = string.Empty;
        public bool IsExempt { get; set; } = false;
        public DateTime? BirthDate { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}
