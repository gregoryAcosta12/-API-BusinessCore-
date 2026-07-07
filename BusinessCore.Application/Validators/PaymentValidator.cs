using BusinessCore.Application.DTOs.Payment;
using BusinessCore.Application.DTOs.Payments;
using BusinessCore.Application.Interfaces;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class PaymentCreateValidator : AbstractValidator<PaymentCreateDto>
    {
        public PaymentCreateValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("El ID de orden es obligatorio");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("El método de pago es obligatorio")
                .MaximumLength(50).WithMessage("El método de pago no puede exceder 50 caracteres");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0")
                .PrecisionScale(18, 2).WithMessage("El monto debe tener hasta 2 decimales");

            RuleFor(x => x.TransactionId)
                .MaximumLength(100).WithMessage("El ID de transacción no puede exceder 100 caracteres");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres");
        }
    }

    public class PaymentUpdateValidator : AbstractValidator<PaymentUpdateDto>
    {
        public PaymentUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("El estado no es válido");

            RuleFor(x => x.TransactionId)
                .MaximumLength(100).WithMessage("El ID de transacción no puede exceder 100 caracteres");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres");
        }
    }

    public class PaymentProcessValidator : AbstractValidator<PaymentProcessDto>
    {
        public PaymentProcessValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("El ID de orden es obligatorio");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("El método de pago es obligatorio")
                .MaximumLength(50).WithMessage("El método de pago no puede exceder 50 caracteres");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0")
                .PrecisionScale(18, 2).WithMessage("El monto debe tener hasta 2 decimales");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("El número de tarjeta es obligatorio")
                .Matches(@"^\d{15,16}$").When(x => x.PaymentMethod == "CreditCard")
                .WithMessage("El número de tarjeta debe tener 15 o 16 dígitos");

            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("El nombre del titular es obligatorio")
                .When(x => x.PaymentMethod == "CreditCard");

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty().WithMessage("El mes de expiración es obligatorio")
                .Matches(@"^(0[1-9]|1[0-2])$").WithMessage("El mes debe ser entre 01 y 12")
                .When(x => x.PaymentMethod == "CreditCard");

            RuleFor(x => x.ExpiryYear)
                .NotEmpty().WithMessage("El año de expiración es obligatorio")
                .Matches(@"^\d{4}$").WithMessage("El año debe tener 4 dígitos")
                .When(x => x.PaymentMethod == "CreditCard");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("El CVV es obligatorio")
                .Matches(@"^\d{3,4}$").WithMessage("El CVV debe tener 3 o 4 dígitos")
                .When(x => x.PaymentMethod == "CreditCard");
        }
    }
}