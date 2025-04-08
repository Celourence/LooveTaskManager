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

namespace LooveTaskManager.Tests.Services;

public class CreateTaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<ILogger<CreateTaskService>> _loggerMock;
    private readonly IValidator<TaskItem> _taskValidator;
    private readonly CreateTaskService _service;

    public CreateTaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _loggerMock = new Mock<ILogger<CreateTaskService>>();
        _taskValidator = new TaskValidator();
        _service = new CreateTaskService(_taskRepositoryMock.Object, _taskValidator, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_ShouldCreateTask()
    {
        // Arrange
        var request = new CreateTaskRequestDTO
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = (int)Domain.Enums.TaskStatus.Pending
        };

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        _taskRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem task) => task);

        // Act
        var result = await _service.ExecuteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.DueDate, result.DueDate);
        Assert.Equal((Domain.Enums.TaskStatus)request.Status, result.Status);
        
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithDuplicateTitle_ShouldThrowConflictException()
    {
        // Arrange
        var request = new CreateTaskRequestDTO
        {
            Title = "Existing Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = (int)Domain.Enums.TaskStatus.Pending
        };

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(request));
        
        Assert.Equal(Messages.Error.Task.TitleAlreadyExists, exception.Message);
        Assert.Equal(ErrorCodes.Task.TitleAlreadyExists, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithPastDueDate_ShouldThrowValidationException()
    {
        // Arrange
        var request = new CreateTaskRequestDTO
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(-1),
            Status = (int)Domain.Enums.TaskStatus.Pending
        };

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(request));
        
        Assert.Equal(ErrorMessages.Task.PastDueDate, exception.Message);
        Assert.Equal(ErrorCodes.General.ValidationError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithRepositoryError_ShouldThrowInternalException()
    {
        // Arrange
        var request = new CreateTaskRequestDTO
        {
            Title = "New Task",
            Description = "Task description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = (int)Domain.Enums.TaskStatus.Pending
        };

        _taskRepositoryMock.Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        _taskRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .ThrowsAsync(new Exception("Database save error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(request));
        
        Assert.Equal(Messages.Error.Database.SaveError, exception.Message);
        Assert.Equal(ErrorCodes.General.InternalServerError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.ExistsByTitleAsync(request.Title), Times.Once);
        _taskRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    }
} 