using BusinessCore.Application.DTOs.Brand;
using BusinessCore.Application.DTOs.Brands;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class BrandCreateValidator : AbstractValidator<BrandCreateDto>
    {
        public BrandCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la marca es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Website)
                .MaximumLength(200).WithMessage("El sitio web no puede exceder 200 caracteres")
                .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")
                .When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage("El sitio web no tiene un formato válido");
        }
    }

    public class BrandUpdateValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la marca es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Website)
                .MaximumLength(200).WithMessage("El sitio web no puede exceder 200 caracteres");
        }
    }
}