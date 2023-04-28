using Application.DTOs;
using Application.MarketingTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WCMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseApiController
    {
        public TaskController(IMediator mediator) : base(mediator)
        {

        }
        
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {  
            return HandleResult(await _mediator.Send(new GetTaskList.Query()));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubTask(string id)
        {
            return HandleResult(await _mediator.Send(new GetTask.Query { TaskId = id }));
        }

        [Authorize(Roles = "manager")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]MarketingTaskDTO marketing)
        {
            return HandleResult(await _mediator.Send(new CreateTask.Command { MarketingTask = marketing }));
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateTask([FromBody] MarketingTaskDTO marketing)
        {
            return HandleResult(await _mediator.Send(new UpdateTask.Command { MarketingTask = marketing }));
        }

        

    }
}
