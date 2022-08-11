using Application.Campaigns;
using Core.Campaigns;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public CampaignController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Ok(await _mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new Detail.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Campaign campaign)
        {
            await _mediator.Send(new Create.Command { Campaign = campaign });

            return Ok();
        }
    }
}
