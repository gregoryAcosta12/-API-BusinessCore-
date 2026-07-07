using BusinessCore.Application.DTOs.Product;
using BusinessCore.Application.DTOs.Products;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres");

            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("El SKU es obligatorio")
                .MaximumLength(50).WithMessage("El SKU no puede exceder 50 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
                .PrecisionScale(18, 2).WithMessage("El precio debe tener hasta 2 decimales");

            RuleFor(x => x.CostPrice)
                .GreaterThan(0).When(x => x.CostPrice.HasValue)
                .WithMessage("El precio de costo debe ser mayor a 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");

            RuleFor(x => x.MinStock)
                .GreaterThanOrEqualTo(0).When(x => x.MinStock.HasValue)
                .WithMessage("El stock mínimo no puede ser negativo");

            RuleFor(x => x.MaxStock)
                .GreaterThanOrEqualTo(0).When(x => x.MaxStock.HasValue)
                .WithMessage("El stock máximo no puede ser negativo");

            RuleFor(x => x.Weight)
                .GreaterThan(0).When(x => x.Weight.HasValue)
                .WithMessage("El peso debe ser mayor a 0");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).When(x => x.CategoryId.HasValue)
                .WithMessage("El ID de categoría debe ser válido");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).When(x => x.BrandId.HasValue)
                .WithMessage("El ID de marca debe ser válido");

            When(x => x.Images != null, () =>
            {
                RuleForEach(x => x.Images).SetValidator(new ProductImageCreateValidator());
            });

            When(x => x.Variants != null, () =>
            {
                RuleForEach(x => x.Variants).SetValidator(new ProductVariantCreateValidator());
            });
        }
    }

    public class ProductUpdateValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres");

            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("El SKU es obligatorio")
                .MaximumLength(50).WithMessage("El SKU no puede exceder 50 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
                .PrecisionScale(18, 2).WithMessage("El precio debe tener hasta 2 decimales");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");
        }
    }

    public class ProductImageCreateValidator : AbstractValidator<ProductImageCreateDto>
    {
        public ProductImageCreateValidator()
        {
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("La URL de la imagen es obligatoria")
                .MaximumLength(500).WithMessage("La URL no puede exceder 500 caracteres");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("El orden de visualización no puede ser negativo");
        }
    }

    public class ProductVariantCreateValidator : AbstractValidator<ProductVariantCreateDto>
    {
        public ProductVariantCreateValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("El SKU de la variante es obligatorio")
                .MaximumLength(50).WithMessage("El SKU no puede exceder 50 caracteres");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la variante es obligatorio")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
                .PrecisionScale(18, 2).WithMessage("El precio debe tener hasta 2 decimales");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");
        }
    }
}