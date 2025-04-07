using FluentValidation;
using LooveTaskManager.Domain.Common.Constants;
using LooveTaskManager.Domain.Entities;

namespace LooveTaskManager.Domain.Validators;

public class TaskValidator : AbstractValidator<TaskItem>
{
    public TaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ErrorMessages.Task.EmptyTitle)
            .MaximumLength(200)
            .WithMessage(ErrorMessages.Task.TitleTooLong);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(ErrorMessages.Task.EmptyDescription);

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(ErrorMessages.Task.PastDueDate);
    }
} 