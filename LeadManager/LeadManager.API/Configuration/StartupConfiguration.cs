using LeadManager.API.BusinessLogic.Common;
using LeadManager.Core.Entities;
using LeadManager.Core.Interfaces.Lead;
using LeadManager.Core.Interfaces.Source;
using LeadManager.Core.Interfaces.Supplier;
using LeadManager.Core.Interfaces;
using LeadManager.Infrastructure.Data.Repositories;
using LeadManager.Infrastructure.Data;
using LeadManager.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace LeadManager.API.Configuration
{
    public static class StartupConfiguration
    {
        public static void RegisterCustomDependencies(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<DbSeeder>();
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
            builder.Services.AddTransient<IEmailService, TestEmailService>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<ISourceRepository, SourceRepository>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<ILeadService, LeadService>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<IApiResponseHandler, ApiResponseHandler>();
            builder.Services.AddScoped<ILeadRepository, LeadRepository>();
            builder.Services.AddScoped<ILeadTypeRepository, LeadTypeRepository>();
            builder.Services.AddScoped<ILeadAttributeRepository, LeadAttributeRepository>();

            builder.Services.AddDbContext<LeadManagerDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:LeadManagerDb"]));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt")
                .CreateLogger();
        }

        public static void ConfigureIdentityAndAuthentication(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<LeadManagerDbContext>();

            builder.Services.AddScoped<PasswordValidator<User>>();

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
        }

        public static void ConfigureControllers(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
            {
                //If Accept header value in the request is not supported, give back a 406 response saying it is not supported
                options.ReturnHttpNotAcceptable = true;
                options.Filters.Add<HttpResponseExceptionFilter>();
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
        }
    }
}
