using Application.Interface;
using Infrastructure.External.Clouds;
using Infrastructure.External.Email;
using Infrastructure.External.SMS;
using Infrastructure.External.Web;
using Infrastructure.Interface;
using Infrastructure.Services;
using Infrastructure.Services.Campaigns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            //add infrastructure twilio

            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<ISMSService, TwilioSMS>();
            services.AddScoped<ISendSMTPClient, SendSMTPClient>();
            services.AddScoped<IWebPostService, WebPostService>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            
            services.AddScoped<IUserAccessorService, UserAccessorService>();
            services.AddScoped<IUploadCsvImportContacts, UploadCsvImportContacts>();

            return services;
        }
    }
}
