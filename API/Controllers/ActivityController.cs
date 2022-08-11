using Application.Activities;
using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sms")]
        public async Task<IActionResult> SendSMS([FromBody]SmsDTO smsFormValue)
        {
            await _mediator.Send(new SendSMS.Command { SmsFormValue = smsFormValue });

            return Ok();
        }
    }
}
