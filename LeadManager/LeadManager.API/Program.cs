using LeadManager.API.BusinessLogic.Common;
using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Interfaces;
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

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();

// Add services to the container.
//You could also do this using Services.AddMvc/Services.AddMvcCore/Services.AddControllersWithViews,
//However these will add additional services that are not required for an API such as Razor view support 

builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<LeadManagerDbContext>();

builder.Services.AddScoped<PasswordValidator<User>>();

builder.Services.AddTransient<DbSeeder>();

builder.Services.AddAuthentication()
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = builder.Configuration["Tokens:Issuer"],
            ValidAudience = builder.Configuration["Tokens:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
        };
    }
    );

builder.Services.AddControllers(options =>
{
    //If Accept header value in the request is not supported, give back a 406 response saying it is not supported
    options.ReturnHttpNotAcceptable = true; 
} 
).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    //Uncomment below section to remove camel case output format (default)
    //if (options.SerializerSettings.ContractResolver != null)
    //{
    //    var castedResolver = options.SerializerSettings.ContractResolver as DefaultContractResolver;
    //    castedResolver.NamingStrategy = null;
    //}

}
            )
.AddXmlDataContractSerializerFormatters(); //Add support for responses in XML format 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddTransient<IEmailService, TestEmailService>();
builder.Services.AddScoped<ISourceService, SourceService>();
builder.Services.AddScoped<IApiEndpointValidation, EndpointValidation>();
builder.Services.AddScoped<ILeadInfoRepository, LeadInfoRepository>();
builder.Services.AddDbContext<LeadManagerDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:LeadManagerDb"]));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

