using FluentValidation;
using PortfolioAPI.Application.Commands;

namespace PortfolioAPI.Application.Validators
{
    public class CreatePortfolioCommandValidator : AbstractValidator<CreatePortfolioCommand>
    {
        public CreatePortfolioCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Portfolio name is required")
                .MaximumLength(100).WithMessage("Portfolio name cannot exceed 100 characters");
        }
    }
} 