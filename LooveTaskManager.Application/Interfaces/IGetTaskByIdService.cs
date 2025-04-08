using LooveTaskManager.Application.DTOs;

namespace LooveTaskManager.Application.Interfaces;
public interface IGetTaskByIdService
{
    Task<TaskResponseDTO> ExecuteAsync(Guid id);
} 