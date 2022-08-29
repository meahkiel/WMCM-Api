using Application.Configuration;
using Infrastructure.Repositories.Customers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence.Context;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {

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

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
                
            });

            //repository
            services.AddScoped<ICustomerRepo, CustomerRepo>();

            services.AddMediatR(typeof(Application.Campaigns.List.QueryHandler));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
                opt.InputFormatters.Insert(0, new CsvInputFormatter());
            });

            return services;
        }
    }
}
