using System.Net;
using LooveTaskManager.Application.Constants;
using LooveTaskManager.Application.Services;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Common.Exceptions;
using LooveTaskManager.Domain.Entities;
using LooveTaskManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace LooveTaskManager.Tests.Services;

public class DeleteTaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<ILogger<DeleteTaskService>> _loggerMock;
    private readonly DeleteTaskService _service;

    public DeleteTaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _loggerMock = new Mock<ILogger<DeleteTaskService>>();
        _service = new DeleteTaskService(_taskRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingTask_ShouldDeleteTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = TaskItem.Create(
            "Test Task",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _taskRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<TaskItem>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.ExecuteAsync(taskId);

        // Assert
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.DeleteAsync(existingTask), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingTask_ShouldThrowNotFoundException()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((TaskItem?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId));
        
        Assert.Equal(Messages.Error.Task.NotFound, exception.Message);
        Assert.Equal(ErrorCodes.Task.TaskNotFound, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithRepositoryError_ShouldThrowInternalException()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = TaskItem.Create(
            "Test Task",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _taskRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<TaskItem>()))
            .ThrowsAsync(new Exception("Database delete error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(taskId));
        
        Assert.Equal(Messages.Error.Database.DeleteError, exception.Message);
        Assert.Equal(ErrorCodes.General.InternalServerError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        _taskRepositoryMock.Verify(x => x.DeleteAsync(existingTask), Times.Once);
    }
} 