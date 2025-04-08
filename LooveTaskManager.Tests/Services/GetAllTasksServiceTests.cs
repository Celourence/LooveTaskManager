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

public class GetAllTasksServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<ILogger<GetAllTasksService>> _loggerMock;
    private readonly GetAllTasksService _service;

    public GetAllTasksServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _loggerMock = new Mock<ILogger<GetAllTasksService>>();
        _service = new GetAllTasksService(_taskRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_ShouldReturnTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            TaskItem.Create("Task 1", "Description 1", DateTime.UtcNow.AddDays(1), Domain.Enums.TaskStatus.Pending),
            TaskItem.Create("Task 2", "Description 2", DateTime.UtcNow.AddDays(2), Domain.Enums.TaskStatus.InProgress)
        };

        var skip = 0;
        var take = 10;
        var totalCount = tasks.Count;

        _taskRepositoryMock.Setup(x => x.GetAllAsync(skip, take))
            .ReturnsAsync(tasks);

        _taskRepositoryMock.Setup(x => x.GetTotalCountAsync())
            .ReturnsAsync(totalCount);

        // Act
        var result = await _service.ExecuteAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(totalCount, result.Total);
        Assert.Equal(tasks.Count, result.Items.Count);
        Assert.Equal(tasks[0].Title, result.Items[0].Title);
        Assert.Equal(tasks[1].Title, result.Items[1].Title);
        Assert.Equal(skip, result.Skip);
        Assert.Equal(take, result.Take);
        
        _taskRepositoryMock.Verify(x => x.GetAllAsync(skip, take), Times.Once);
        _taskRepositoryMock.Verify(x => x.GetTotalCountAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var tasks = new List<TaskItem>();
        var skip = 0;
        var take = 10;
        var totalCount = 0;

        _taskRepositoryMock.Setup(x => x.GetAllAsync(skip, take))
            .ReturnsAsync(tasks);

        _taskRepositoryMock.Setup(x => x.GetTotalCountAsync())
            .ReturnsAsync(totalCount);

        // Act
        var result = await _service.ExecuteAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.Total);
        Assert.Equal(skip, result.Skip);
        Assert.Equal(take, result.Take);
        
        _taskRepositoryMock.Verify(x => x.GetAllAsync(skip, take), Times.Once);
        _taskRepositoryMock.Verify(x => x.GetTotalCountAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldThrowInternalServerError()
    {
        // Arrange
        var skip = 0;
        var take = 10;

        _taskRepositoryMock.Setup(x => x.GetAllAsync(skip, take))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _service.ExecuteAsync(skip, take));
        
        Assert.Equal(Messages.Error.Database.QueryError, exception.Message);
        Assert.Equal(ErrorCodes.General.InternalServerError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
        
        _taskRepositoryMock.Verify(x => x.GetAllAsync(skip, take), Times.Once);
        _taskRepositoryMock.Verify(x => x.GetTotalCountAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithDefaultParameters_ShouldUseDefaultValues()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            TaskItem.Create("Task 1", "Description 1", DateTime.UtcNow.AddDays(1), Domain.Enums.TaskStatus.Pending)
        };
        var total = 1;

        _taskRepositoryMock.Setup(x => x.GetAllAsync(0, 10))
            .ReturnsAsync(tasks);
        
        _taskRepositoryMock.Setup(x => x.GetTotalCountAsync())
            .ReturnsAsync(total);

        // Act
        var result = await _service.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(total, result.Total);
        Assert.Equal(0, result.Skip);
        Assert.Equal(10, result.Take);
        Assert.Single(result.Items);
        
        _taskRepositoryMock.Verify(x => x.GetAllAsync(0, 10), Times.Once);
        _taskRepositoryMock.Verify(x => x.GetTotalCountAsync(), Times.Once);
    }
} 