using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.Queries;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PortfoliosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<PortfolioDto>> CreatePortfolio([FromBody] CreatePortfolioCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPortfolio), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<PortfolioDto>>> GetPortfolios()
        {
            var result = await _mediator.Send(new GetAllPortfoliosQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PortfolioDto>> GetPortfolio(Guid id)
        {
            var result = await _mediator.Send(new GetPortfolioQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PortfolioDto>> UpdatePortfolio(Guid id, [FromBody] UpdatePortfolioCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePortfolio(Guid id)
        {
            var result = await _mediator.Send(new DeletePortfolioCommand { Id = id });
            return Ok(new { success = result });
        }

        [HttpGet("{id}/performance")]
        public async Task<ActionResult<PortfolioPerformanceDto>> GetPortfolioPerformance(
            Guid id, 
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _mediator.Send(new GetPortfolioPerformanceQuery 
            { 
                PortfolioId = id, 
                StartDate = startDate, 
                EndDate = endDate 
            });
            return Ok(result);
        }

        [HttpGet("{id}/allocation")]
        public async Task<ActionResult<AssetAllocationDto>> GetAssetAllocation(Guid id)
        {
            var result = await _mediator.Send(new GetAssetAllocationQuery { PortfolioId = id });
            return Ok(result);
        }

        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<List<TransactionDto>>> GetTransactionHistory(
            Guid id, 
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _mediator.Send(new GetTransactionHistoryQuery 
            { 
                PortfolioId = id, 
                StartDate = startDate, 
                EndDate = endDate 
            });
            return Ok(result);
        }
    }
} 