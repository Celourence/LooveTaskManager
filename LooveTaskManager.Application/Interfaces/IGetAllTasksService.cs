using LooveTaskManager.Application.DTOs;

namespace LooveTaskManager.Application.Interfaces;

public interface IGetAllTasksService
{
    Task<TaskListResponseDTO> ExecuteAsync(int skip = 0, int take = 10);
} 