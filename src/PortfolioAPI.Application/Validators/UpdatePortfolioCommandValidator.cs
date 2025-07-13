using FluentValidation;
using PortfolioAPI.Application.Commands;

namespace PortfolioAPI.Application.Validators
{
    public class UpdatePortfolioCommandValidator : AbstractValidator<UpdatePortfolioCommand>
    {
        public UpdatePortfolioCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Portfolio ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Portfolio name is required")
                .MaximumLength(100).WithMessage("Portfolio name cannot exceed 100 characters");
        }
    }
} 