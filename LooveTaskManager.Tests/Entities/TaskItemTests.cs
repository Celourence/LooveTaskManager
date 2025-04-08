using Xunit;
using LooveTaskManager.Domain.Entities;

namespace LooveTaskManager.Tests.Entities;

public class TaskItemTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTaskItem()
    {
        // Arrange
        var title = "Test Task";
        var description = "Test Description";
        var dueDate = DateTime.UtcNow.AddDays(1);
        var status = Domain.Enums.TaskStatus.Pending;

        // Act
        var task = TaskItem.Create(title, description, dueDate, status);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(dueDate, task.DueDate);
        Assert.Equal(status, task.Status);
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.True(task.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Create_WithoutStatus_ShouldCreateTaskWithPendingStatus()
    {
        // Arrange
        var title = "Test Task";
        var description = "Test Description";
        var dueDate = DateTime.UtcNow.AddDays(1);

        // Act
        var task = TaskItem.Create(title, description, dueDate);

        // Assert
        Assert.Equal(Domain.Enums.TaskStatus.Pending, task.Status);
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateTaskItem()
    {
        // Arrange
        var task = TaskItem.Create(
            "Original Title",
            "Original Description",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        var newTitle = "Updated Title";
        var newDescription = "Updated Description";
        var newDueDate = DateTime.UtcNow.AddDays(2);
        var newStatus = Domain.Enums.TaskStatus.InProgress;

        // Act
        task.Update(newTitle, newDescription, newDueDate, newStatus);

        // Assert
        Assert.Equal(newTitle, task.Title);
        Assert.Equal(newDescription, task.Description);
        Assert.Equal(newDueDate, task.DueDate);
        Assert.Equal(newStatus, task.Status);
    }

    [Fact]
    public void UpdateStatus_WithValidStatus_ShouldUpdateTaskStatus()
    {
        // Arrange
        var task = TaskItem.Create(
            "Test Task",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        var newStatus = Domain.Enums.TaskStatus.Completed;

        // Act
        task.UpdateStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, task.Status);
    }
} 