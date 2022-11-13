using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
