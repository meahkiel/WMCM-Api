using Application.Activities;
using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    
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
        public async Task<IActionResult> SendSMS([FromBody]ActivityEntryDTO smsFormValue)
        {
            smsFormValue.Type = "sms";
            return HandleResult(await _mediator.Send(new SendActivity.Command { Entry = smsFormValue }));
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody]ActivityEntryDTO emailFormValue)
        {
            emailFormValue.Type = "email";
            return HandleResult(await _mediator.Send(new SendActivity.Command { Entry = emailFormValue }));
        }
    }
}
