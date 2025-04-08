using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Domain.Entities;
using Mapster;

namespace LooveTaskManager.Application.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        TypeAdapterConfig<TaskItem, TaskResponseDTO>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.DueDate, src => src.DueDate)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<CreateTaskRequestDTO, TaskItem>.NewConfig()
            .ConstructUsing(src => TaskItem.Create(
                src.Title,
                src.Description ?? string.Empty,
                src.DueDate,
                (Domain.Enums.TaskStatus)src.Status));
    }
} 