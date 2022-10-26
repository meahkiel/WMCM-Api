using Application.Channels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : BaseApiController
    {

        public ChannelController(IMediator mediator) : 
            base(mediator)
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChannels()
        {
            return HandleResult(
                await _mediator.Send(new All.Query()));
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetChannel([FromRoute]string id)
        {

            return HandleResult(await _mediator
                                        .Send(new GetById.Query{ Id = id}));

        }


    }
}
