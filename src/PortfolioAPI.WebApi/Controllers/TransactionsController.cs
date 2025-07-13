using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> RecordTransaction([FromBody] RecordTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(RecordTransaction), new { id = result.Id }, result);
        }
    }
} 