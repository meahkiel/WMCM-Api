using Application.Configuration;
using Application.Extensions;
using Core.Users;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Persistence.Context;
using Persistence.Seeds;
using WCMAPI.Extensions;
using WCMAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDefaultServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
.AddIdentityServices(builder.Configuration);

builder.Services.Configure<TwilioOption>(builder.Configuration.GetSection("TwilioSMSKey"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestSwagger", Version = "v1" });
    c.CustomSchemaIds(x => x.FullName);
});




var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapFallbackToController("Index","Fallback");
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await Seed.DataSeedChannel(context);
    await Seed.DataSeedTemplate(context);
    //await Seed.DataSeedContact(context);
    await Seed.SeedRoleData(context,userManager,roleManager);

}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}
           

app.Run();
