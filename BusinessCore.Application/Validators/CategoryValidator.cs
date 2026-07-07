using BusinessCore.Application.DTOs.Categories;
using BusinessCore.Application.DTOs.Category;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("El slug es obligatorio")
                .MaximumLength(100).WithMessage("El slug no puede exceder 100 caracteres")
                .Matches("^[a-z0-9-]+$").WithMessage("El slug solo puede contener letras minúsculas, números y guiones");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("El orden de visualización no puede ser negativo");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(0).When(x => x.ParentCategoryId.HasValue)
                .WithMessage("El ID de categoría padre debe ser válido");
        }
    }

    public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("El slug es obligatorio")
                .MaximumLength(100).WithMessage("El slug no puede exceder 100 caracteres")
                .Matches("^[a-z0-9-]+$").WithMessage("El slug solo puede contener letras minúsculas, números y guiones");
        }
    }
}