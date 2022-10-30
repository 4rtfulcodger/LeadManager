var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//You could also do this using Services.AddMvc/Services.AddMvcCore/Services.AddControllersWithViews,
//However these will add additional services that are not required for an API such as Razor view support 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
