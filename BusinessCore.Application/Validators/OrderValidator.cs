using BusinessCore.Application.DTOs.Order;
using BusinessCore.Application.DTOs.Orders;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class OrderCreateValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio");

            RuleFor(x => x.AddressId)
                .GreaterThan(0).WithMessage("La dirección de envío es obligatoria");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("La orden debe tener al menos un item")
                .Must(x => x.Count > 0).WithMessage("La orden debe tener al menos un item");

            RuleFor(x => x.Currency)
                .MaximumLength(3).WithMessage("La moneda debe tener 3 caracteres");

            RuleForEach(x => x.Items).SetValidator(new OrderItemCreateValidator());
        }
    }

    public class OrderUpdateValidator : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("El estado no es válido");

            RuleFor(x => x.TrackingNumber)
                .MaximumLength(100).WithMessage("El número de seguimiento no puede exceder 100 caracteres");

            RuleFor(x => x.ShippingMethod)
                .MaximumLength(100).WithMessage("El método de envío no puede exceder 100 caracteres");
        }
    }

    public class OrderItemCreateValidator : AbstractValidator<OrderItemCreateDto>
    {
        public OrderItemCreateValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("El ID de producto es obligatorio");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("El precio unitario no puede ser negativo")
                .PrecisionScale(18, 2).WithMessage("El precio debe tener hasta 2 decimales");

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("El descuento no puede ser negativo")
                .LessThanOrEqualTo(x => x.UnitPrice).WithMessage("El descuento no puede ser mayor al precio unitario");
        }
    }
}