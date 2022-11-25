using Application.Interface;
using Application.SeedWorks;
using Infrastructure.External.Clouds;
using Infrastructure.External.Credential;
using Infrastructure.External.Email;
using Infrastructure.External.SMS;
using Infrastructure.External.Web;
using Infrastructure.Interface;
using Infrastructure.Services;
using Infrastructure.Services.Campaigns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class DefaultServiceExtension
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services,
            IConfiguration configuration)
        {

            //add infrastructure twilio
            services.Configure<TwilioSettings>(configuration.GetSection("TwilioSMSKey"));
            services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:3000");
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
}
