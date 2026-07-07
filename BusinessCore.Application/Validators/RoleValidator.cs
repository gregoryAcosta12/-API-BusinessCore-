using BusinessCore.Application.DTOs.Roles;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class RoleCreateValidator : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio")
                .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres");
        }
    }

    public class RoleUpdateValidator : AbstractValidator<RoleUpdateDto>
    {
        public RoleUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio")
                .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres");
        }
    }
}