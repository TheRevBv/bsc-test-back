using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BSC.Infrastructure.Persistences.Contexts;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Infrastructure.Persistences.Repositories;

namespace BSC.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(BSCContext).Assembly.FullName;

            services.AddDbContext<BSCContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("BSCConnection"), b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}