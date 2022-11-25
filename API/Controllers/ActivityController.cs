using Application.Activities.Commands;
using Application.Activities.Queries;
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
            return HandleResult(await _mediator.Send(new HandleActivity.Command { Activity = smsFormValue }));
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody]ActivityEntryDTO emailFormValue)
        {
            emailFormValue.Type = "email";
            return HandleResult(await _mediator.Send(new HandleActivity.Command { Activity = emailFormValue }));
        }

        [HttpPost("web")]
        public async Task<IActionResult> SendWeb([FromForm]ActivityEntryDTO webForm)
        {
            webForm.Type = "web";
            return HandleResult(await _mediator.Send(new HandleActivity.Command { Activity = webForm }));

        }
    }
}
