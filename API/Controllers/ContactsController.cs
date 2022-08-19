using Application.Contacts;
using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    
    public class ContactsController : BaseApiController
    {   
        public ContactsController(IMediator mediator) : 
            base(mediator)
        {
           
        }

        
        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] List<ContactFormDTO> contacts)
        {   
            return HandleResult(await _mediator.Send(new Import.Command { Entries = contacts } ));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ContactFormDTO form )
        {
            return HandleResult(await _mediator.Send(new Create.Command { ContactForm = form}));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ContactFormDTO form)
        {
            return HandleResult(await _mediator
                    .Send(new Update.Command { ContactForm = form }));
        }

        
        [HttpGet]
        public async Task<IActionResult> All()
        {
            return HandleResult(await _mediator.Send(new List.Query()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Edit([FromRoute] Find.Query query)
        {
            return HandleResult(await _mediator.Send(query));
        }



    }
}
