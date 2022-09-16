﻿using Application.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Repositories.Unit;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(typeof(Application.Campaigns.List.QueryHandler));
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
            });

            services.AddAutoMapper(typeof(MappingProfiles));
            //repository
            services.AddScoped<UnitWrapper>();
            
            return services;
        }
    }
}