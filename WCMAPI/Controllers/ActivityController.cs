using Application.Activities.Commands;
using Application.Activities.Queries;
using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WCMAPI.Controllers
{

    [Route("api/[controller]")]
    public class ActivityController : BaseApiController
    {
      
        public ActivityController(IMediator mediator) : base(mediator)
        {
           
        }

        [HttpGet("initial/{type}")]
        public async Task<IActionResult> Index(string type)
        {
            return HandleResult(await _mediator.Send(new Initialize.Query(type)));
        }

        
        [HttpGet("replay/{id}")]
        public async Task<IActionResult> Replay([FromRoute]ExecuteActivity.Command cmd)
        {
            return HandleResult(await _mediator.Send(cmd));
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteActivity.Command cmd)
        {
            return HandleResult(await _mediator.Send(cmd));
        }


        [AllowAnonymous]
        [HttpGet("page")]
        public async Task<IActionResult> GetActivity()
        {
            return HandleResult(await _mediator.Send(new GetActivityByType.Query())); 
        }
       

        [HttpPost("sms")]
        public async Task<IActionResult> SendSMS([FromBody]ActivityEntryDTO smsFormValue)
        {
            smsFormValue.Type = "sms";
            return HandleResult(await _mediator.Send(new HandleActivity.Command (smsFormValue)));
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody]ActivityEntryDTO emailFormValue)
        {
            emailFormValue.Type = "email";
            return HandleResult(await _mediator.Send(new HandleActivity.Command (emailFormValue)));
        }

        [HttpPost("web")]
        public async Task<IActionResult> SendWeb([FromForm]ActivityEntryDTO webForm)
        {
            webForm.Type = "web";
            return HandleResult(await _mediator.Send(new HandleActivity.Command (webForm)));

        }

        [HttpPost("social")]
        public async Task<IActionResult> SendSocial([FromBody]ActivityEntryDTO socialFormValue)
        {
            socialFormValue.Type = "social";
            return HandleResult(await _mediator.Send(new HandleActivity.Command (socialFormValue)));
        }
    }
}
