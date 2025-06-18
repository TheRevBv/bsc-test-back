using Microsoft.Extensions.DependencyInjection;
using BSC.Api.Extensions;
using BSC.Application.Extensions;
using BSC.Infrastructure.Extensions;
using BSC.Utilities.AppSettings;
using WatchDog;


namespace BSC.Api;

public class Startup
{
    [Obsolete]
    public void Initialize(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var Configuration = builder.Configuration;
        // Add services to the container.
        var Cors = "AngularApp";
        var listUris = Configuration.GetSection("Cors:Uri").Get<string[]>();

        builder.Services.AddInjectionInfraestructure(Configuration);
        builder.Services.AddInjectionApplication(Configuration);
        builder.Services.AddAuthentication(Configuration);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();

        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("GoogleSettings"));

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: Cors,
                builder =>
                {
                    builder.WithOrigins(listUris!)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition");
                });
        });

        var app = builder.Build();

        app.UseCors(Cors);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseWatchDogExceptionLogger();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.UseWatchDog(configuration =>
        {
            configuration.WatchPageUsername = Configuration.GetSection("WatchDog:Username").Value;
            configuration.WatchPagePassword = Configuration.GetSection("WatchDog:Password").Value;
        });

        app.Run();
    }
}