using FluentValidation;
using Customers.Domain.Dtos;
using Customers.Domain.Enums;

namespace Customers.Application.Validators
{
    public class DeleteCustomerDtoValidator : AbstractValidator<DeleteCustomerDto>
    {
        public DeleteCustomerDtoValidator()
        {
            RuleFor(model => model.Id)
            .GreaterThan(0).WithMessage("ID inválido");
        }
    }
}
