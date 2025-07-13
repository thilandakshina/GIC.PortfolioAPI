using FluentValidation;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.Interfaces;

namespace PortfolioAPI.Application.Validators
{
    public class RecordTransactionCommandValidator : AbstractValidator<RecordTransactionCommand>
    {
        public RecordTransactionCommandValidator(ITransactionRepository transactionRepository)
        {
            RuleFor(x => x.PortfolioId)
                .NotEmpty().WithMessage("Portfolio ID is required");

            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("Asset ID is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future");

            When(x => x.TransactionType == Domain.TransactionType.Sell, () =>
            {
                RuleFor(x => x)
                    .MustAsync(async (command, cancellation) =>
                    {
                        var currentQuantity = await transactionRepository.GetAssetQuantityInPortfolioAsync(command.PortfolioId, command.AssetId);
                        return currentQuantity >= command.Quantity;
                    })
                    .WithMessage("Cannot sell more shares than owned");
            });
        }
    }
} 