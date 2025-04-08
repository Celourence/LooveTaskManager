using System.Net;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Common.Exceptions;
using LooveTaskManager.Domain.Interfaces;
using LooveTaskManager.Application.Interfaces;
using Microsoft.Extensions.Logging;
using LooveTaskManager.Application.Constants;

namespace LooveTaskManager.Application.Services;

public class DeleteTaskService : IDeleteTaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<DeleteTaskService> _logger;

    public DeleteTaskService(
        ITaskRepository taskRepository,
        ILogger<DeleteTaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(Guid id)
    {
        try
        {
            _logger.LogInformation(string.Format(Messages.Log.Task.DeletingTask, id));

            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                _logger.LogWarning(string.Format(Messages.Log.Task.TaskNotFound, id));
                throw new CustomException(
                    Messages.Error.Task.NotFound,
                    ErrorCodes.Task.TaskNotFound,
                    HttpStatusCode.NotFound);
            }

            await _taskRepository.DeleteAsync(task);
            _logger.LogInformation(string.Format(Messages.Log.Task.TaskDeleted, id));
        }
        catch (CustomException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Task.ErrorDeletingTask);
            throw new CustomException(
                Messages.Error.Database.DeleteError,
                ErrorCodes.General.InternalServerError,
                HttpStatusCode.InternalServerError);
        }
    }
} 