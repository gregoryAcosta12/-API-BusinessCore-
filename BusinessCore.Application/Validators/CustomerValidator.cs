using BusinessCore.Application.DTOs.Customer;
using BusinessCore.Application.DTOs.Customers;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class CustomerCreateValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("El nombre de la empresa es obligatorio")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

            RuleFor(x => x.TaxId)
                .NotEmpty().WithMessage("El RFC/CUIT/NIT es obligatorio")
                .MaximumLength(50).WithMessage("El TaxId no puede exceder 50 caracteres");

            RuleFor(x => x.BusinessType)
                .MaximumLength(100).WithMessage("El tipo de negocio no puede exceder 100 caracteres");

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0).WithMessage("El límite de crédito no puede ser negativo")
                .PrecisionScale(18, 2).WithMessage("El límite de crédito debe tener hasta 2 decimales");

            RuleFor(x => x.PaymentTerms)
                .MaximumLength(100).WithMessage("Los términos de pago no pueden exceder 100 caracteres");
        }
    }

    public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("El nombre de la empresa es obligatorio")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

            RuleFor(x => x.TaxId)
                .NotEmpty().WithMessage("El RFC/CUIT/NIT es obligatorio")
                .MaximumLength(50).WithMessage("El TaxId no puede exceder 50 caracteres");

            RuleFor(x => x.BusinessType)
                .MaximumLength(100).WithMessage("El tipo de negocio no puede exceder 100 caracteres");

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0).WithMessage("El límite de crédito no puede ser negativo");

            RuleFor(x => x.PaymentTerms)
                .MaximumLength(100).WithMessage("Los términos de pago no pueden exceder 100 caracteres");
        }
    }
}