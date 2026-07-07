using BusinessCore.Application.DTOs.Inventory;
using BusinessCore.Domain.Enums;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class InventoryMovementCreateValidator : AbstractValidator<InventoryMovementCreateDto>
    {
        public InventoryMovementCreateValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("El ID de producto es obligatorio");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("El tipo de movimiento no es válido");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");

            RuleFor(x => x.UnitCost)
                .GreaterThanOrEqualTo(0).WithMessage("El costo unitario no puede ser negativo")
                .PrecisionScale(18, 2).WithMessage("El costo debe tener hasta 2 decimales");

            RuleFor(x => x.Reference)
                .MaximumLength(100).WithMessage("La referencia no puede exceder 100 caracteres");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres");

            RuleFor(x => x.SourceWarehouseId)
                .GreaterThan(0).When(x => x.Type == MovementType.Transfer)
                .WithMessage("El almacén de origen es obligatorio para transferencias");

            RuleFor(x => x.TargetWarehouseId)
                .GreaterThan(0).When(x => x.Type == MovementType.Transfer)
                .WithMessage("El almacén de destino es obligatorio para transferencias");

            RuleFor(x => x)
                .Must(x => x.SourceWarehouseId != x.TargetWarehouseId)
                .When(x => x.Type == MovementType.Transfer)
                .WithMessage("El almacén de origen y destino deben ser diferentes");
        }
    }
}