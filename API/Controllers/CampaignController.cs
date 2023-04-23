using Application.Campaigns.Commands;
using Application.Campaigns.Queries;
using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : BaseApiController
    {

        public CampaignController(IMediator mediator) : base(mediator)
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return HandleResult(await _mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await _mediator.Send(new Detail.Query { Id = id }));
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]Create.Command campaign)
        {
            var unit = await _mediator.Send(campaign);
            return HandleResult(unit);
        }


        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] Edit.Command command)
        {
            var unit = await _mediator.Send(command);
            return HandleResult(unit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var unit = await _mediator.Send(new Delete.Command { Id = id });
            
            return HandleResult(unit);
        }

        
    }
}
