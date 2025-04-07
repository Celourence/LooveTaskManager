namespace LooveTaskManager.Application.DTOs;

/// <summary>
/// DTO de resposta com os dados de uma tarefa
/// </summary>
public record TaskResponseDTO
{
    /// <summary>
    /// Identificador único da tarefa
    /// </summary>
    /// <example>77aa1d87-8c0d-4b09-aeda-341e6d38a79a</example>
    public required Guid Id { get; init; }

    /// <summary>
    /// Título da tarefa
    /// </summary>
    /// <example>Implementar autenticação</example>
    public required string Title { get; init; }

    /// <summary>
    /// Descrição detalhada da tarefa
    /// </summary>
    /// <example>Implementar autenticação usando JWT com refresh token</example>
    public string? Description { get; init; }

    /// <summary>
    /// Data de vencimento da tarefa
    /// </summary>
    /// <example>2025-12-31T23:59:59Z</example>
    public required DateTime DueDate { get; init; }

    /// <summary>
    /// Status atual da tarefa
    /// </summary>
    /// <example>Pending</example>
    public required Domain.Enums.TaskStatus Status { get; init; }

    /// <summary>
    /// Data de criação da tarefa
    /// </summary>
    /// <example>2025-04-07T18:21:10.6451072Z</example>
    public required DateTime CreatedAt { get; init; }
} 