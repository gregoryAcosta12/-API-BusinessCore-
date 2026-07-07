using BusinessCore.Application.DTOs.Review;
using BusinessCore.Application.DTOs.Reviews;
using FluentValidation;

namespace BusinessCore.Application.Validators
{
    public class ReviewCreateValidator : AbstractValidator<ReviewCreateDto>
    {
        public ReviewCreateValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("El ID de producto es obligatorio");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("La calificación debe ser entre 1 y 5");

            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("El comentario es obligatorio")
                .MaximumLength(1000).WithMessage("El comentario no puede exceder 1000 caracteres");
        }
    }

    public class ReviewUpdateValidator : AbstractValidator<ReviewUpdateDto>
    {
        public ReviewUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser válido");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("La calificación debe ser entre 1 y 5");

            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("El comentario es obligatorio")
                .MaximumLength(1000).WithMessage("El comentario no puede exceder 1000 caracteres");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("El estado no es válido");
        }
    }
}