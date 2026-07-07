using BusinessCore.Application.DTOs.Address;
using BusinessCore.Application.DTOs.Addresses;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class AddressCreateValidator : AbstractValidator<AddressCreateDto>
    {
        public AddressCreateValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("La calle es obligatoria")
                .MaximumLength(200).WithMessage("La calle no puede exceder 200 caracteres");

            RuleFor(x => x.Number)
                .MaximumLength(20).WithMessage("El número no puede exceder 20 caracteres");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad es obligatoria")
                .MaximumLength(100).WithMessage("La ciudad no puede exceder 100 caracteres");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("El estado es obligatorio")
                .MaximumLength(100).WithMessage("El estado no puede exceder 100 caracteres");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("El código postal es obligatorio")
                .MaximumLength(20).WithMessage("El código postal no puede exceder 20 caracteres");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("El país es obligatorio")
                .MaximumLength(100).WithMessage("El país no puede exceder 100 caracteres");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres");

            RuleFor(x => x.AddressType)
                .MaximumLength(20).WithMessage("El tipo de dirección no puede exceder 20 caracteres");
        }
    }

    public class AddressUpdateValidator : AbstractValidator<AddressUpdateDto>
    {
        public AddressUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("La calle es obligatoria")
                .MaximumLength(200).WithMessage("La calle no puede exceder 200 caracteres");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad es obligatoria")
                .MaximumLength(100).WithMessage("La ciudad no puede exceder 100 caracteres");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("El estado es obligatorio")
                .MaximumLength(100).WithMessage("El estado no puede exceder 100 caracteres");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("El código postal es obligatorio")
                .MaximumLength(20).WithMessage("El código postal no puede exceder 20 caracteres");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("El país es obligatorio")
                .MaximumLength(100).WithMessage("El país no puede exceder 100 caracteres");
        }
    }
}