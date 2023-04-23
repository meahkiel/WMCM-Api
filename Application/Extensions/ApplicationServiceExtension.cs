using Application.Campaigns.Queries;
using Application.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Repositories.Unit;
using System.Reflection;

namespace Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddMediatR(typeof(List.QueryHandler));
            
            services.AddDbContext<DataContext>(opt =>
            {
                //opt.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
                opt.UseNpgsql(configuration.GetConnectionString("PostgreConnection"));
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MappingProfiles));
            //repository
            services.AddScoped<UnitWrapper>();
            
            return services;
        }
    }
}
