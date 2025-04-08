namespace LooveTaskManager.Application.Interfaces;

public interface IDeleteTaskService
{
    Task ExecuteAsync(Guid id);
} 