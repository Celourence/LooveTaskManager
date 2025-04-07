using LooveTaskManager.Application.DTOs;

namespace LooveTaskManager.Application.Interfaces;

public interface ICreateTaskService
{
    Task<TaskResponseDTO> ExecuteAsync(CreateTaskRequestDTO request);
} 