using System.Net;
using FluentValidation;
using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Common.Exceptions;
using LooveTaskManager.Domain.Entities;
using LooveTaskManager.Domain.Interfaces;
using LooveTaskManager.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Mapster;
using LooveTaskManager.Application.Constants;

namespace LooveTaskManager.Application.Services;

public class UpdateTaskService : IUpdateTaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IValidator<TaskItem> _taskValidator;
    private readonly ILogger<UpdateTaskService> _logger;

    public UpdateTaskService(
        ITaskRepository taskRepository,
        IValidator<TaskItem> taskValidator,
        ILogger<UpdateTaskService> logger)
    {
        _taskRepository = taskRepository;
        _taskValidator = taskValidator;
        _logger = logger;
    }

    public async Task<TaskResponseDTO> ExecuteAsync(Guid id, UpdateTaskRequestDTO request)
    {
        try
        {
            _logger.LogInformation(string.Format(Messages.Log.Task.UpdatingTask, id));

            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                _logger.LogWarning(string.Format(Messages.Log.Task.TaskNotFound, id));
                throw new CustomException(
                    Messages.Error.Task.NotFound,
                    ErrorCodes.Task.TaskNotFound,
                    HttpStatusCode.NotFound);
            }

            if (await _taskRepository.ExistsByTitleAsync(request.Title) && task.Title != request.Title)
            {
                _logger.LogWarning(string.Format(Messages.Log.Task.DuplicateTitle, request.Title));
                throw new CustomException(
                    Messages.Error.Task.TitleAlreadyExists,
                    ErrorCodes.Task.TitleAlreadyExists,
                    HttpStatusCode.Conflict);
            }

            task.Update(
                request.Title,
                request.Description ?? string.Empty,
                request.DueDate,
                (Domain.Enums.TaskStatus)request.Status);

            var validationResult = await _taskValidator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(Messages.Log.Task.ValidationFailed);
                throw new CustomException(
                    validationResult.Errors.First().ErrorMessage,
                    ErrorCodes.General.ValidationError,
                    HttpStatusCode.BadRequest);
            }

            await _taskRepository.UpdateAsync(task);
            _logger.LogInformation(string.Format(Messages.Log.Task.TaskUpdated, task.Id));

            return task.Adapt<TaskResponseDTO>();
        }
        catch (CustomException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Task.ErrorUpdatingTask);
            throw new CustomException(
                Messages.Error.Database.SaveError,
                ErrorCodes.Database.SaveError,
                HttpStatusCode.InternalServerError);
        }
    }
} 