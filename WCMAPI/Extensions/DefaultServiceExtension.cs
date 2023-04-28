using Application.SeedWorks;
using Infrastructure.External.Credential;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;

namespace WCMAPI.Extensions;

public static class DefaultServiceExtension
{
    public static IServiceCollection AddDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {

        //add infrastructure twilio
        services.Configure<TwilioSettings>(configuration.GetSection("TwilioSMSKey"));
        services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));
        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
        
     

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
        });

        services.AddControllers(opt =>
        {
            var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
            opt.Filters.Add(new AuthorizeFilter(policy));
            opt.InputFormatters.Add(new CsvInputFormatter());
        });

        return services;
    }
}
