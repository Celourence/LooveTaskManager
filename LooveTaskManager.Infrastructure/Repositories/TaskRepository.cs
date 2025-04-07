using LooveTaskManager.Domain.Entities;
using LooveTaskManager.Domain.Interfaces;
using LooveTaskManager.Infrastructure.Data;
using LooveTaskManager.Application.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace LooveTaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TaskRepository> _logger;

    public TaskRepository(ApplicationDbContext context, ILogger<TaskRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<TaskItem> Query() => _context.Tasks;

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.AddingEntity);
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation(Messages.Log.Repository.EntityAdded);
            return task;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorAddingTask);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(Expression<Func<TaskItem, bool>> predicate)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.CheckingEntityExists);
            return await _context.Tasks.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorGettingTasks);
            throw;
        }
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(int skip = 0, int take = 10)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.GettingAllEntities);
            var tasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            
            return tasks.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorGettingTasks);
            throw;
        }
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.GettingEntityById);
            return await _context.Tasks.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorGettingTasks);
            throw;
        }
    }

    public async Task UpdateAsync(TaskItem task)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.UpdatingEntity);
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation(Messages.Log.Repository.EntityUpdated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorUpdatingTask);
            throw;
        }
    }

    public async Task DeleteAsync(TaskItem task)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.RemovingEntity);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation(Messages.Log.Repository.EntityRemoved);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorDeletingTask);
            throw;
        }
    }

    public async Task<int> CountAsync()
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.CountingTasks);
            return await _context.Tasks.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorGettingTasks);
            throw;
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.GettingTotalTasks);
            return await _context.Tasks.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorGettingTasks);
            throw;
        }
    }

    public async Task<bool> ExistsByTitleAsync(string title)
    {
        try
        {
            _logger.LogInformation(Messages.Log.Repository.CheckingTitleExists);
            return await _context.Tasks.AnyAsync(t => t.Title == title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.Log.Repository.ErrorGettingTasks);
            throw;
        }
    }
} 