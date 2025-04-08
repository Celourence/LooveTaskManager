using System.Net;
using FluentValidation;
using LooveTaskManager.Application.Constants;
using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Application.Services;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Common.Exceptions;
using LooveTaskManager.Domain.Entities;
using LooveTaskManager.Domain.Interfaces;
using LooveTaskManager.Domain.Validators;
using Microsoft.Extensions.Logging;
using Moq;
using TaskStatus = LooveTaskManager.Domain.Enums.TaskStatus;

namespace LooveTaskManager.Tests.Services;

public class UpdateTaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<ILogger<UpdateTaskService>> _loggerMock;
    private readonly IValidator<TaskItem> _taskValidator;
    private readonly UpdateTaskService _service;

    public UpdateTaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _loggerMock = new Mock<ILogger<UpdateTaskService>>();
        _taskValidator = new TaskValidator();
        _service = new UpdateTaskService(_taskRepositoryMock.Object, _taskValidator, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_ShouldUpdateTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = TaskItem.Create(
            "Original Title",
            "Original Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatus.Pending);

        var request = new UpdateTaskRequestDTO
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = (int)TaskStatus.InProgress
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        _taskRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.ExecuteAsync(taskId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.DueDate, result.DueDate);
        Assert.Equal((TaskStatus)request.Status, result.Status);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingTask_ShouldThrowNotFoundException()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var request = new UpdateTaskRequestDTO
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = (int)TaskStatus.InProgress
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((TaskItem?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId, request));
        
        Assert.Equal(Messages.Error.Task.NotFound, exception.Message);
        Assert.Equal(ErrorCodes.Task.TaskNotFound, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(It.IsAny<string>()), Times.Never);
        _taskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithDuplicateTitle_ShouldThrowConflictException()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = TaskItem.Create(
            "Original Title",
            "Original Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatus.Pending);

        var request = new UpdateTaskRequestDTO
        {
            Title = "Existing Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = (int)TaskStatus.InProgress
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId, request));
        
        Assert.Equal(Messages.Error.Task.TitleAlreadyExists, exception.Message);
        Assert.Equal(ErrorCodes.Task.TitleAlreadyExists, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithPastDueDate_ShouldThrowValidationException()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = TaskItem.Create(
            "Original Title",
            "Original Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatus.Pending);

        var request = new UpdateTaskRequestDTO
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(-1),
            Status = (int)TaskStatus.InProgress
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId, request));
        
        Assert.Equal(ErrorMessages.Task.PastDueDate, exception.Message);
        Assert.Equal(ErrorCodes.General.ValidationError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithRepositoryError_ShouldThrowInternalException()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = TaskItem.Create(
            "Original Title",
            "Original Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatus.Pending);

        var request = new UpdateTaskRequestDTO
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = (int)TaskStatus.InProgress
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        _taskRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId, request));
        
        Assert.Equal(Messages.Error.Database.SaveError, exception.Message);
        Assert.Equal(ErrorCodes.Database.SaveError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
    }
} 