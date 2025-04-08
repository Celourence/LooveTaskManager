using LooveTaskManager.API.Middlewares;
using LooveTaskManager.Infrastructure.IoC;
using LooveTaskManager.Application.Mappings;
using Microsoft.OpenApi.Models;
using LooveTaskManager.API.Configuration;

namespace LooveTaskManager.API;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Loove Task Manager API", Version = "v1" });
            c.EnableAnnotations();
        });

        services.AddInfrastructureServices(Configuration);

        MappingConfig.RegisterMappings();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loove Task Manager API V1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseAuthorization();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
} 