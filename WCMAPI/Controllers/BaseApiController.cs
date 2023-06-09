﻿using Application.SeedWorks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WCMAPI.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices
            .GetService<IMediator>();

        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();

            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            
            return BadRequest(result.Error);
        }

       
    }
}
