using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); //cloudinarysettings is the name we gave in aoosettings.json
            services.AddScoped<ITokenService, TokenService>(); //scoped last as long as http request last
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options =>
            {  //here we inicialize which base we're using and set connection string
                options.UseSqlite(config.GetConnectionString("DefaultConnection")); //here we pass on the config we set in config json file
            }); //DataContext is the class taht we created in API.Data
        return services;
        }
    }
}