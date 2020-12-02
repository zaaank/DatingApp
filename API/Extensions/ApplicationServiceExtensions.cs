using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){
            services.AddScoped<ITokenService, TokenService>(); //scoped last as long as http request last
            services.AddDbContext<DataContext>(options =>
            {  //here we inicialize which base we're using and set connection string
                options.UseSqlite(config.GetConnectionString("DefaultConnection")); //here we pass on the config we set in config json file
            }); //DataContext is the class taht we created in API.Data
        return services;
        }
    }
}