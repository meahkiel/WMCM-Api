
using Application.Core;
using Infrastructure.External.Credential;
using Infrastructure.External.Email;
using Infrastructure.External.SMS;
using Infrastructure.Services;
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
            services.AddScoped<ISMSService, TwilioSMS>();
            services.AddScoped<ISendSMTPClient,SendSMTPClient>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IUserAccessorService, UserAccessorService>();
            services.AddScoped<IUploadCsvImportContacts, UploadCsvImportContacts>();

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
