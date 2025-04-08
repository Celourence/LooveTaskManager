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

public class CreateTaskService : ICreateTaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IValidator<TaskItem> _taskValidator;
    private readonly ILogger<CreateTaskService> _logger;

    public CreateTaskService(
        ITaskRepository taskRepository,
        IValidator<TaskItem> taskValidator,
        ILogger<CreateTaskService> logger)
    {
        _taskRepository = taskRepository;
        _taskValidator = taskValidator;
        _logger = logger;
    }

    public async Task<TaskResponseDTO> ExecuteAsync(CreateTaskRequestDTO request)
    {
        try
        {
            _logger.LogInformation(string.Format(Messages.Log.Task.CreatingTask, request.Title));

            if (await _taskRepository.ExistsByTitleAsync(request.Title))
            {
                _logger.LogWarning(string.Format(Messages.Log.Task.DuplicateTitle, request.Title));
                throw new CustomException(
                    Messages.Error.Task.TitleAlreadyExists,
                    ErrorCodes.Task.TitleAlreadyExists,
                    HttpStatusCode.Conflict);
            }

            var task = TaskItem.Create(
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

            await _taskRepository.AddAsync(task);
            _logger.LogInformation(string.Format(Messages.Log.Task.TaskCreated, task.Id));

            return task.Adapt<TaskResponseDTO>();
        }
        catch (CustomException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Task.ErrorCreatingTask);
            throw new CustomException(
                Messages.Error.Database.SaveError,
                ErrorCodes.General.InternalServerError,
                HttpStatusCode.InternalServerError);
        }
    }
} 