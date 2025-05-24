using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BSC.Application.Commons.Ordering;
using BSC.Application.Extensions.WatchDog;
using BSC.Application.Interfaces;
using BSC.Application.Services;
using System.Reflection;

namespace BSC.Application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<IOrderingQuery, OrderingQuery>();
            services.AddScoped<IUsuarioApplication, UsuarioApplication>(); ;
            services.AddScoped<IAuthApplication, AuthApplication>();
            services.AddScoped<IProductoApplication, ProductoApplication>();
            services.AddScoped<IPedidoApplication, PedidoApplication>();
            services.AddScoped<IRolApplication, RolApplication>();

            services.AddTransient<IFileStorageLocalApplication, FileStorageLocalApplication>();
            services.AddScoped<IGenerateExcelApplication, GenerateExcelApplication>();
            services.AddScoped<ISqlCoreApplication, SqlCoreApplication>();

            services.AddWatchDog(configuration);

            return services;
        }
    }
}