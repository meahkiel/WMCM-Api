
using Application.Contacts;
using Application.DTO;
using Infrastructure.Core;
using Infrastructure.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{

    public class ContactsController : BaseApiController
    {

        private readonly IUploadCsvImportContacts _uploadFileDL;

        public ContactsController(IMediator mediator, IUploadCsvImportContacts uploadFileDL) : 
            base(mediator)
        {
            _uploadFileDL = uploadFileDL;
        }

        
        [HttpPost, Route("import")]
        public async Task<IActionResult> Import([FromForm]UploadCsvFileRequest request)
        {   
            string path = "Resources/" + request.File.FileName;
            
            return HandleResult(await _uploadFileDL.UploadCsvFile(request, path));

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

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody]Delete.Command cmd)
        {
            return HandleResult(await _mediator.Send(cmd));
        }

        

    }
}
