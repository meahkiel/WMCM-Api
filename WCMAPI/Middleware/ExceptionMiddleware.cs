using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WCMAPI.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, 
        ILogger<ExceptionMiddleware> logger, 
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ValidationException ex)
        {
            _logger.LogError(ex, ex.Message);
            var problemDetails = GetBadRequestValidationProblemDetails(ex);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(problemDetails, options);

            await context.Response.WriteAsync(json);
        }
       
        catch(Exception ex2)
        {



            _logger.LogError(ex2, ex2.Message);
            var response = GetServerProblemDetails(ex2);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);

        }

    }

    private ValidationProblemDetails GetBadRequestValidationProblemDetails(ValidationException ex)
    {
        string traceId = Guid.NewGuid().ToString();
        var errors = new Dictionary<string, string[]>();

        foreach (var error in ex.Errors)
        {
            errors.Add(error.PropertyName, new string[] { error.ErrorMessage });
        }

        var validationProblemDetails = new ValidationProblemDetails(errors);

        validationProblemDetails.Status = (int)HttpStatusCode.BadRequest;
        validationProblemDetails.Type = "https://httpstatuses.com/400";
        validationProblemDetails.Title = "Validation failed";
        validationProblemDetails.Detail = "One or more inputs need to be corrected. Check errors for details";
        validationProblemDetails.Instance = traceId;

        return validationProblemDetails;
    }

    private ValidationProblemDetails GetServerProblemDetails(Exception ex)
    {
        string traceId = Guid.NewGuid().ToString();
        var errors = new Dictionary<string, string[]>();
        

        var validationProblemDetails = new ValidationProblemDetails(errors);

        validationProblemDetails.Status = (int)HttpStatusCode.InternalServerError;
        validationProblemDetails.Type = "https://httpstatuses.com/500";
        validationProblemDetails.Title = "Internal Server Error";
        validationProblemDetails.Detail = ex.StackTrace?.ToString();
        validationProblemDetails.Instance = traceId;

        return validationProblemDetails;
    }
}


