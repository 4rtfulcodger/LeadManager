using LeadManager.Core.Interfaces;
using LeadManager.Infrastructure.Services;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

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
builder.Services.AddControllers(options =>
{
    //If Accept header value in the request is not supported, give back a 406 response saying it is not supported
    options.ReturnHttpNotAcceptable = true; 
} 
).AddXmlDataContractSerializerFormatters(); //Add support for responses in XML format 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddTransient<IEmailService, TestEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/healthcheck");
});
app.MapControllers();

app.Run();

//In normal circumstances the Program.cs is compiled into a private Program class that cannot be accessed out of this assembly
//We add the following partial class so we can access Program from the IntegrationTests project
public partial class Program { }