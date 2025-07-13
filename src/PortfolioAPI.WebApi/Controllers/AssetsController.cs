using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.Queries;
using PortfolioAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAPI.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<AssetDto>> CreateAsset([FromBody] CreateAssetCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAsset), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<AssetDto>>> GetAssets()
        {
            var result = await _mediator.Send(new GetAllAssetsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetDto>> GetAsset(Guid id)
        {
            var result = await _mediator.Send(new GetAssetQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AssetDto>> UpdateAsset(Guid id, [FromBody] UpdateAssetCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsset(Guid id)
        {
            var result = await _mediator.Send(new DeleteAssetCommand { Id = id });
            return Ok(new { success = result });
        }
    }
} 