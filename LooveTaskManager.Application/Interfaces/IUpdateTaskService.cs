using LooveTaskManager.Application.DTOs;

namespace LooveTaskManager.Application.Interfaces;

public interface IUpdateTaskService
{
    Task<TaskResponseDTO> ExecuteAsync(Guid id, UpdateTaskRequestDTO request);
} 