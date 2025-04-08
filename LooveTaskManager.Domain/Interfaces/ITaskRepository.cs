using LooveTaskManager.Domain.Entities;

namespace LooveTaskManager.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TaskItem>> GetAllAsync(int skip = 0, int take = 10);
    Task<int> GetTotalCountAsync();
    Task<bool> ExistsByTitleAsync(string title);
    Task<TaskItem> AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
} 