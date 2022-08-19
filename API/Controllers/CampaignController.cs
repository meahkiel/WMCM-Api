using Application.Campaigns;
using Application.DTO;
using Core.Campaigns;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CampaignDTO campaign)
        {
            var unit = await _mediator.Send(new Create.Command { Campaign = campaign });

            return HandleResult(unit);
        }

        
    }
}
