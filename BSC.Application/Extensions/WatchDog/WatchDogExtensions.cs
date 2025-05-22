using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchDog;
using WatchDog.src.Enums;

namespace BSC.Application.Extensions.WatchDog
{
    public static class WatchDogExtensions
    {
        public static IServiceCollection AddWatchDog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWatchDogServices(options =>
            {
                //options.SetExternalDbConnString = configuration.GetConnectionString("BSCConnection");
                //options.SqlDriverOption = WatchDogSqlDriverEnum.MSSQL;
                options.IsAutoClear = true;
                options.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Daily;
            });

            return services;
        }
    }
}