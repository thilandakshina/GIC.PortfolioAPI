using FluentValidation;
using PortfolioAPI.Application.Commands;

namespace PortfolioAPI.Application.Validators
{
    public class UpdateAssetCommandValidator : AbstractValidator<UpdateAssetCommand>
    {
        public UpdateAssetCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Asset ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Asset name is required")
                .MaximumLength(100).WithMessage("Asset name cannot exceed 100 characters");

            RuleFor(x => x.CurrentPrice)
                .GreaterThan(0).WithMessage("Current price must be greater than 0");
        }
    }
} 