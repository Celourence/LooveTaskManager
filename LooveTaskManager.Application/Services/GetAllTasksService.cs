using System.Net;
using LooveTaskManager.Application.Constants;
using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Application.Interfaces;
using LooveTaskManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Common.Exceptions;

namespace LooveTaskManager.Application.Services;

public class GetAllTasksService : IGetAllTasksService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<GetAllTasksService> _logger;

    public GetAllTasksService(
        ITaskRepository taskRepository,
        ILogger<GetAllTasksService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<TaskListResponseDTO> ExecuteAsync(int skip = 0, int take = 10)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Task.GettingTasks);

            var tasks = await _taskRepository.GetAllAsync(skip, take);
            var total = await _taskRepository.GetTotalCountAsync();

            _logger.LogInformation(string.Format(Messages.Log.Task.TasksRetrieved, total));

            return new TaskListResponseDTO
            {
                Items = tasks.Select(t => new TaskResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                }).ToList(),
                Total = total,
                Skip = skip,
                Take = take
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Task.ErrorGettingTasks);
            throw new CustomException(
                Messages.Error.Database.QueryError,
                ErrorCodes.General.InternalServerError,
                HttpStatusCode.InternalServerError);
        }
    }
} 