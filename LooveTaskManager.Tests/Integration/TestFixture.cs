using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using LooveTaskManager.API;
using LooveTaskManager.Infrastructure.Data;
using LooveTaskManager.Application.Interfaces;
using LooveTaskManager.Application.Services;
using LooveTaskManager.Domain.Interfaces;
using LooveTaskManager.Infrastructure.Repositories;
using FluentValidation;
using LooveTaskManager.Domain.Validators;
using LooveTaskManager.Domain.Entities;

namespace LooveTaskManager.Tests.Integration;

public class TestFixture : WebApplicationFactory<Program>
{
    private static string _databaseName = $"TestDb_{Guid.NewGuid()}";
    private IServiceProvider? _serviceProvider;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remover o contexto do banco de dados real
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Adicionar o contexto do banco de dados em memória
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            // Registrar os serviços
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ICreateTaskService, CreateTaskService>();
            services.AddScoped<IGetAllTasksService, GetAllTasksService>();
            services.AddScoped<IGetTaskByIdService, GetTaskByIdService>();
            services.AddScoped<IUpdateTaskService, UpdateTaskService>();
            services.AddScoped<IDeleteTaskService, DeleteTaskService>();
            services.AddScoped<IValidator<TaskItem>, TaskValidator>();

            // Criar o banco de dados e aplicar as migrações
            _serviceProvider = services.BuildServiceProvider();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        });
    }

    public void ResetDatabase()
    {
        if (_serviceProvider != null)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Tasks.RemoveRange(db.Tasks);
            db.SaveChanges();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _serviceProvider != null)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureDeleted();
        }

        base.Dispose(disposing);
    }
} 