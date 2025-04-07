using System.Net;
using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Application.Interfaces;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Common.Exceptions;
using LooveTaskManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Mapster;
using LooveTaskManager.Application.Constants;

namespace LooveTaskManager.Application.Services;

public class GetTaskByIdService : IGetTaskByIdService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<GetTaskByIdService> _logger;

    public GetTaskByIdService(
        ITaskRepository taskRepository,
        ILogger<GetTaskByIdService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<TaskResponseDTO> ExecuteAsync(Guid id)
    {
        try
        {
            _logger.LogInformation(string.Format(Messages.Log.Task.GettingTaskById, id));

            var task = await _taskRepository.GetByIdAsync(id);
            if (task is null)
            {
                _logger.LogWarning(string.Format(Messages.Log.Task.TaskNotFound, id));
                throw new CustomException(
                    Messages.Error.Task.TaskNotFound,
                    ErrorCodes.Task.TaskNotFound,
                    HttpStatusCode.NotFound);
            }

            _logger.LogInformation("Tarefa obtida com sucesso");

            return task.Adapt<TaskResponseDTO>();
        }
        catch (CustomException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, string.Format(Messages.Log.Task.ErrorGettingTaskById, id));
            throw new CustomException(
                Messages.Error.Database.QueryError,
                ErrorCodes.General.InternalServerError,
                HttpStatusCode.InternalServerError);
        }
    }
} 