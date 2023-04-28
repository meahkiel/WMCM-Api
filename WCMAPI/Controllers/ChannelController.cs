using Application.Channels;
using Application.DTO;
using Core.Channels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WCMAPI.Controllers
{
    [Route("api/[controller]")]

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ChannelSettingDTO channelSetting)
        {
            var command = new Create.Command(channelSetting);
            return HandleResult(await _mediator.Send(command));
        }


    }
}
