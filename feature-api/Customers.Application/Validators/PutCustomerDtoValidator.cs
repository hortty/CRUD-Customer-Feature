using FluentValidation;
using Customers.Domain.Dtos;
using Customers.Domain.Enums;

namespace Customers.Application.Validators
{
    public class PutCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public PutCustomerDtoValidator()
        {
            RuleFor(model => model.Id)
                .GreaterThan(0).WithMessage("ID inválido");

            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Nome ou Razão Social não pode estar vazio")
                .MaximumLength(150).WithMessage("Nome ou Razão Social deve ter no máximo 150 caracteres");

            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("E-mail não pode estar vazio")
                .EmailAddress().WithMessage("E-mail inválido")
                .MaximumLength(150).WithMessage("E-mail deve ter no máximo 150 caracteres");

            RuleFor(model => model.Phone)
                .NotEmpty().WithMessage("Telefone não pode estar vazio")
                .Matches(@"^\d{10,11}$").WithMessage("Telefone deve ter entre 10 e 11 dígitos numéricos");

            RuleFor(model => model.PersonType)
                .NotEmpty().WithMessage("Campo tipo Pessoa não pode estar vazio");

            RuleFor(model => model.CpfCnpj)
                .NotEmpty().WithMessage("CPF ou CNPJ não pode estar vazio")
                .When(model => model.PersonType == nameof(PersonType.Fisica))
                .Matches(@"^\d{11}$").WithMessage("CPF deve ter 11 dígitos")
                .When(model => model.PersonType == nameof(PersonType.Fisica));

            RuleFor(model => model.CpfCnpj)
                .NotEmpty().WithMessage("CPF ou CNPJ não pode estar vazio")
                .When(model => model.PersonType == nameof(PersonType.Juridica))
                .Matches(@"^\d{14}$").WithMessage("CNPJ deve ter 14 dígitos")
                .When(model => model.PersonType == nameof(PersonType.Juridica));

            RuleFor(model => model.BirthDate)
                .NotEmpty().WithMessage("Data de Nascimento é obrigatória para pessoas físicas")
                .When(model => model.PersonType == nameof(PersonType.Fisica))
                .WithMessage("Data de Nascimento é obrigatória para pessoas físicas");

            RuleFor(model => model.BirthDate)
                .Empty().WithMessage("Data de Nascimento deve estar vazia para CNPJ")
                .When(model => model.PersonType == nameof(PersonType.Juridica));

            RuleFor(model => model.Gender)
                .NotEmpty().WithMessage("Gênero não pode estar vazio")
                .When(model => model.PersonType == nameof(PersonType.Fisica))
                .WithMessage("Gênero é obrigatório para pessoas físicas");

            RuleFor(model => model.Gender)
                .Empty().WithMessage("Gênero deve estar vazio para CNPJ")
                .When(model => model.PersonType == nameof(PersonType.Juridica));

            RuleFor(model => model.PasswordHash)
                .NotEmpty().WithMessage("Senha não pode estar vazia")
                .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres")
                .MaximumLength(15).WithMessage("Senha deve ter no máximo 15 caracteres");
        }
    }
}
