namespace LooveTaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public Domain.Enums.TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private TaskItem() { }

    public static TaskItem Create(string title, string description, DateTime dueDate, Domain.Enums.TaskStatus status = Domain.Enums.TaskStatus.Pending)
    {
        return new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            DueDate = dueDate,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string title, string description, DateTime dueDate, Domain.Enums.TaskStatus status)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
    }

    public void UpdateStatus(Domain.Enums.TaskStatus newStatus)
    {
        Status = newStatus;
    }
} 