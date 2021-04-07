using CrispArchitecture.Application.Contracts.v1.Customers;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities.Identity;
using CrispArchitecture.Infrastructure.Data;
using CrispArchitecture.Infrastructure.Data.Repository;
using CrispArchitecture.Infrastructure.Identity;
using CrispArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CrispArchitecture.Api
{
    // Class for configuring all services for the API, so the Startup class doesn't get too big. 
    public static class StartupExtensions
    {
        public static void ConfigureDataStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    opt =>
                        opt.MigrationsAssembly("CrispArchitecture.Infrastructure"));
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    opt => 
                        opt.MigrationsAssembly("CrispArchitecture.Infrastructure"));
            });
        }

        public static void ConfigureIdentityServices(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            
            services.AddAuthentication();
        }

        public static void ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IIdentityService, IdentityService>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CrispArchitecture.Api", Version = "v1" });
            });
        }
    }
}