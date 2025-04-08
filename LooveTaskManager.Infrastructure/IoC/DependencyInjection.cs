using FluentValidation;
using LooveTaskManager.Application.Services;
using LooveTaskManager.Application.Interfaces;
using LooveTaskManager.Domain.Interfaces;
using LooveTaskManager.Domain.Validators;
using LooveTaskManager.Infrastructure.Data;
using LooveTaskManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LooveTaskManager.Infrastructure.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ICreateTaskService, CreateTaskService>();
        services.AddScoped<IGetAllTasksService, GetAllTasksService>();
        services.AddScoped<IGetTaskByIdService, GetTaskByIdService>();
        services.AddScoped<IUpdateTaskService, UpdateTaskService>();
        services.AddScoped<IDeleteTaskService, DeleteTaskService>();
        services.AddScoped<IValidator<Domain.Entities.TaskItem>, TaskValidator>();

        return services;
    }
} 