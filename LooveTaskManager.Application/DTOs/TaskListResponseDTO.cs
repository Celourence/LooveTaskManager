namespace LooveTaskManager.Application.DTOs;

/// <summary>
/// DTO para retorno de lista paginada de tarefas
/// </summary>
public record TaskListResponseDTO
{
    /// <summary>
    /// Lista de tarefas
    /// </summary>
    public IReadOnlyList<TaskResponseDTO> Items { get; init; } = new List<TaskResponseDTO>();

    /// <summary>
    /// Total de tarefas
    /// </summary>
    /// <example>10</example>
    public int Total { get; init; }

    /// <summary>
    /// Número de registros pulados na paginação
    /// </summary>
    /// <example>0</example>
    public int Skip { get; init; }

    /// <summary>
    /// Número de registros retornados na página atual
    /// </summary>
    /// <example>10</example>
    public int Take { get; init; }
} 