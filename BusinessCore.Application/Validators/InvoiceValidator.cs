using BusinessCore.Application.DTOs.Invoice;
using BusinessCore.Application.DTOs.Invoices;
using BusinessCore.Application.Interfaces;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class InvoiceCreateValidator : AbstractValidator<InvoiceCreateDto>
    {
        public InvoiceCreateValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("El ID de orden es obligatorio");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("La fecha de vencimiento debe ser futura");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres");
        }
    }

    public class InvoiceUpdateValidator : AbstractValidator<InvoiceUpdateDto>
    {
        public InvoiceUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("La fecha de vencimiento debe ser futura");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("El estado no es válido");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres");

            RuleFor(x => x.AmountPaid)
                .GreaterThanOrEqualTo(0).When(x => x.AmountPaid.HasValue)
                .WithMessage("El monto pagado no puede ser negativo")
                .PrecisionScale(18, 2).WithMessage("El monto debe tener hasta 2 decimales");
        }
    }
}