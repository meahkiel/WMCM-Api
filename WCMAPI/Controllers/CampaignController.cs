using Application.Campaigns.Commands;
using Application.Campaigns.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;

namespace WCMAPI.Controllers
{

    [Route("api/[controller]")]
 
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
        public async Task<IActionResult> EditCampaign([FromBody]Edit.Command command)
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

        [HttpPost("sentiment")]
        public async Task<IActionResult> Sentiment([FromBody]GetSentiment.Query query)
        {
            
            query.FileName = "wwwroot/BrandSentiment.zip";


            return HandleResult(await _mediator.Send(query));
        }

        
    }
}
