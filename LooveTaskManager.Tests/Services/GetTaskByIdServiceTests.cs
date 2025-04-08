using System.Net;
using LooveTaskManager.Application.Constants;
using LooveTaskManager.Application.Services;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Common.Exceptions;
using LooveTaskManager.Domain.Entities;
using LooveTaskManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LooveTaskManager.Tests.Services;

public class GetTaskByIdServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<ILogger<GetTaskByIdService>> _loggerMock;
    private readonly GetTaskByIdService _service;

    public GetTaskByIdServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _loggerMock = new Mock<ILogger<GetTaskByIdService>>();
        _service = new GetTaskByIdService(_taskRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingId_ShouldReturnTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = TaskItem.Create("Test Task", "Test Description", DateTime.UtcNow.AddDays(1), Domain.Enums.TaskStatus.Pending);

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        // Act
        var result = await _service.ExecuteAsync(taskId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(task.Title, result.Title);
        Assert.Equal(task.Description, result.Description);
        Assert.Equal(task.DueDate, result.DueDate);
        Assert.Equal(task.Status, result.Status);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ShouldThrowNotFoundException()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((TaskItem?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId));
        
        Assert.Equal(Messages.Error.Task.TaskNotFound, exception.Message);
        Assert.Equal(ErrorCodes.Task.TaskNotFound, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldThrowInternalServerError()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId));
        
        Assert.Equal(Messages.Error.Database.QueryError, exception.Message);
        Assert.Equal(ErrorCodes.General.InternalServerError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
    }
} 