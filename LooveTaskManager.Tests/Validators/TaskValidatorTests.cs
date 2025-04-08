using FluentValidation;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Entities;
using LooveTaskManager.Domain.Validators;
using Xunit;

namespace LooveTaskManager.Tests.Validators;

public class TaskValidatorTests
{
    private readonly TaskValidator _validator;

    public TaskValidatorTests()
    {
        _validator = new TaskValidator();
    }

    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var task = TaskItem.Create(
            "",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        // Act
        var result = _validator.Validate(task);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TaskItem.Title));
        Assert.Contains(result.Errors, e => e.ErrorMessage == ErrorMessages.Task.EmptyTitle);
    }

    [Fact]
    public void Validate_WhenTitleIsTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var task = TaskItem.Create(
            new string('a', 201),
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        // Act
        var result = _validator.Validate(task);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TaskItem.Title));
        Assert.Contains(result.Errors, e => e.ErrorMessage == ErrorMessages.Task.TitleTooLong);
    }

    [Fact]
    public void Validate_WhenDescriptionIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var task = TaskItem.Create(
            "Test Title",
            "",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        // Act
        var result = _validator.Validate(task);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TaskItem.Description));
        Assert.Contains(result.Errors, e => e.ErrorMessage == ErrorMessages.Task.EmptyDescription);
    }

    [Fact]
    public void Validate_WhenDueDateIsInThePast_ShouldHaveValidationError()
    {
        // Arrange
        var task = TaskItem.Create(
            "Test Title",
            "Test Description",
            DateTime.UtcNow.AddDays(-1),
            Domain.Enums.TaskStatus.Pending);

        // Act
        var result = _validator.Validate(task);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(TaskItem.DueDate));
        Assert.Contains(result.Errors, e => e.ErrorMessage == ErrorMessages.Task.PastDueDate);
    }

    [Fact]
    public void Validate_WhenAllPropertiesAreValid_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var task = TaskItem.Create(
            "Test Title",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            Domain.Enums.TaskStatus.Pending);

        // Act
        var result = _validator.Validate(task);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
} 