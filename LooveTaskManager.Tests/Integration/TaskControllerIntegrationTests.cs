using System.Net;
using System.Net.Http.Json;
using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Domain.Entities;
using Xunit;
using TaskStatus = LooveTaskManager.Domain.Enums.TaskStatus;

namespace LooveTaskManager.Tests.Integration;

public class TaskControllerIntegrationTests : TestBase
{
    public TaskControllerIntegrationTests(TestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Update_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var taskId = await CreateTestTaskAsync();
        var request = new UpdateTaskRequestDTO
        {
            Title = $"Updated Title {Guid.NewGuid()}",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = (int)TaskStatus.InProgress
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/v1/task/{taskId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<TaskResponseDTO>();
        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.DueDate, result.DueDate);
        Assert.Equal((TaskStatus)request.Status, result.Status);
    }

    [Fact]
    public async Task Update_WithNonExistingTask_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingTaskId = Guid.NewGuid();
        var request = new UpdateTaskRequestDTO
        {
            Title = $"Updated Title {Guid.NewGuid()}",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = (int)TaskStatus.InProgress
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/v1/task/{nonExistingTaskId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_WithDuplicateTitle_ShouldReturnConflict()
    {
        // Arrange
        var firstTaskTitle = $"First Task {Guid.NewGuid()}";
        var taskId = await CreateTestTaskAsync(firstTaskTitle);
        var anotherTaskId = await CreateTestTaskAsync($"Another Task {Guid.NewGuid()}");
        var request = new UpdateTaskRequestDTO
        {
            Title = firstTaskTitle,
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = (int)TaskStatus.InProgress
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/v1/task/{anotherTaskId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Update_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var taskId = await CreateTestTaskAsync();
        var request = new UpdateTaskRequestDTO
        {
            Title = $"Updated Title {Guid.NewGuid()}",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(-1),
            Status = (int)TaskStatus.InProgress
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/v1/task/{taskId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithExistingTask_ShouldReturnNoContent()
    {
        // Arrange
        var taskId = await CreateTestTaskAsync();

        // Act
        var response = await Client.DeleteAsync($"/api/v1/task/{taskId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithNonExistingTask_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingTaskId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"/api/v1/task/{nonExistingTaskId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
} 