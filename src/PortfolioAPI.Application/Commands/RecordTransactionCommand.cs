using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Domain;
using System;

namespace PortfolioAPI.Application.Commands
{
    public class RecordTransactionCommand : IRequest<TransactionDto>
    {
        public Guid PortfolioId { get; set; }
        public Guid AssetId { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime TransactionDate { get; set; }
    }
} 