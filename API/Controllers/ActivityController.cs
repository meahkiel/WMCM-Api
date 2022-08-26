using Application.Activities;
using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : BaseApiController
    {
      

        public ActivityController(IMediator mediator) : base(mediator)
        {
           
        }

        [HttpGet("initial/{type}")]
        public async Task<IActionResult> Index(string type)
        {
            return HandleResult(await _mediator.Send(new Initialize.Query { Type = type}));
        }

       

        [HttpPost("sms")]
        public async Task<IActionResult> SendSMS([FromBody]ActivitySMSDTO smsFormValue)
        {
            return HandleResult(await _mediator.Send(new CreateSMSActivity.Command { SMSActivity = smsFormValue }));
        }
    }
}
