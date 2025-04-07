using System.ComponentModel.DataAnnotations;

namespace LooveTaskManager.Application.DTOs;

/// <summary>
/// DTO para criação de uma nova tarefa
/// </summary>
public record CreateTaskRequestDTO
{
    /// <summary>
    /// Título da tarefa
    /// </summary>
    /// <example>Implementar autenticação</example>
    [Required(ErrorMessage = "O título da tarefa é obrigatório")]
    [StringLength(200, ErrorMessage = "O título não pode ter mais que 200 caracteres")]
    public required string Title { get; init; }

    /// <summary>
    /// Descrição detalhada da tarefa
    /// </summary>
    /// <example>Implementar autenticação usando JWT com refresh token</example>
    [StringLength(4000, ErrorMessage = "A descrição não pode ter mais que 4000 caracteres")]
    public string? Description { get; init; }

    /// <summary>
    /// Data de vencimento da tarefa
    /// </summary>
    /// <example>2025-12-31T23:59:59Z</example>
    [Required(ErrorMessage = "A data de vencimento é obrigatória")]
    public required DateTime DueDate { get; init; }

    /// <summary>
    /// Status atual da tarefa
    /// </summary>
    /// <example>0</example>
    [Required(ErrorMessage = "O status da tarefa é obrigatório")]
    [Range(0, 2, ErrorMessage = "O status deve ser 0 (Pendente), 1 (Em Andamento) ou 2 (Concluída)")]
    public required int Status { get; init; }
} 