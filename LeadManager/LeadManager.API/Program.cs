using LeadManager.API.BusinessLogic.Common;
using LeadManager.API.Configuration;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces;
using LeadManager.Core.Interfaces.Lead;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Infrastructure.Data;
using LeadManager.Infrastructure.Data.Repositories;
using LeadManager.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;



StartupConfiguration.ConfigureLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();

// Add services to the container.
//You could also do this using Services.AddMvc/Services.AddMvcCore/Services.AddControllersWithViews,
//However these will add additional services that are not required for an API such as Razor view support 

StartupConfiguration.ConfigureIdentityAndAuthentication(builder);

StartupConfiguration.ConfigureControllers(builder);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();

StartupConfiguration.RegisterCustomDependencies(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting().UseEndpoints(endpoints =>
{
    app.UseAuthorization();
    endpoints.MapHealthChecks("/healthcheck");
});
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<LeadManagerDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var configuration = services.GetRequiredService<IConfiguration>();

    await context.Database.MigrateAsync();
    await DbSeeder.SeedAsync(context, userManager, roleManager, configuration);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error was encountered during  migration");
}

app.Run();


//In normal circumstances the Program.cs is compiled into a private Program class that cannot be accessed out of this assembly
//We add the following partial class so we can access Program from the IntegrationTests project
public partial class Program { }

