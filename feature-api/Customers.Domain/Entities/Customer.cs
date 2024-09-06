using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Customers.Domain.Enums;

namespace Customers.Domain.Entities
{
    [Table("customer")]
    public class Customer : EntityBase
    {
        public Customer() { }

        public Customer(long id, string name, string email, string phone, PersonType personType, string cpfCnpj)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            PersonType = personType;
            CpfCnpj = cpfCnpj;
        }

        [Required]
        [MaxLength(150)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(11)]
        [Column("phone")]
        public string Phone { get; set; }

        [Column("registrationdate")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Column("isblocked")]
        public bool IsBlocked { get; set; } = false;

        [Required]
        [Column("persontype")]
        public PersonType PersonType { get; set; }

        [Required]
        [MaxLength(14)]
        [Column("cpfcnpj")]
        public string CpfCnpj { get; set; }

        [MaxLength(12)]
        [Column("stateregistration")]
        public string StateRegistration { get; set; }

        [Column("isexempt")]
        public bool IsExempt { get; set; } = false;

        [Column("gender")]
        public Gender? Gender { get; set; }

        [Column("birthdate")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(15)]
        [Column("passwordhash")]
        public string PasswordHash { get; set; }

    }
}
