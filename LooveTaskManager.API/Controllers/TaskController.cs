using LooveTaskManager.Application.Constants;
using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LooveTaskManager.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ICreateTaskService _createTaskService;
    private readonly IGetAllTasksService _getAllTasksService;
    private readonly IGetTaskByIdService _getTaskByIdService;
    private readonly IUpdateTaskService _updateTaskService;
    private readonly IDeleteTaskService _deleteTaskService;
    private readonly ILogger<TaskController> _logger;

    public TaskController(
        ICreateTaskService createTaskService,
        IGetAllTasksService getAllTasksService,
        IGetTaskByIdService getTaskByIdService,
        IUpdateTaskService updateTaskService,
        IDeleteTaskService deleteTaskService,
        ILogger<TaskController> logger)
    {
        _createTaskService = createTaskService;
        _getAllTasksService = getAllTasksService;
        _getTaskByIdService = getTaskByIdService;
        _updateTaskService = updateTaskService;
        _deleteTaskService = deleteTaskService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova tarefa
    /// </summary>
    /// <param name="request">Dados da tarefa a ser criada</param>
    /// <returns>Tarefa criada</returns>
    /// <response code="201">Retorna a tarefa criada</response>
    /// <response code="400">Se os dados da tarefa forem inválidos</response>
    /// <response code="409">Se já existir uma tarefa com o mesmo título</response>
    /// <response code="500">Se ocorrer um erro interno</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequestDTO request)
    {
        _logger.LogInformation(string.Format(Messages.Log.Task.CreatingTask, request.Title));
        var result = await _createTaskService.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtém todas as tarefas com paginação
    /// </summary>
    /// <param name="skip">Número de registros a pular</param>
    /// <param name="take">Número de registros a retornar</param>
    /// <returns>Lista de tarefas</returns>
    /// <response code="200">Retorna a lista de tarefas</response>
    /// <response code="500">Se ocorrer um erro interno</response>
    [HttpGet]
    [ProducesResponseType(typeof(TaskListResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllTasks([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        _logger.LogInformation(Messages.Log.Task.GettingTasks);
        var result = await _getAllTasksService.ExecuteAsync(skip, take);
        return Ok(result);
    }

    /// <summary>
    /// Obtém uma tarefa pelo ID
    /// </summary>
    /// <param name="id">ID da tarefa</param>
    /// <returns>Tarefa encontrada</returns>
    /// <response code="200">Retorna a tarefa encontrada</response>
    /// <response code="404">Se a tarefa não for encontrada</response>
    /// <response code="500">Se ocorrer um erro interno</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation(string.Format(Messages.Log.Task.GettingTaskById, id));
        var result = await _getTaskByIdService.ExecuteAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Atualiza uma tarefa existente
    /// </summary>
    /// <param name="id">ID da tarefa</param>
    /// <param name="request">Dados atualizados da tarefa</param>
    /// <returns>Tarefa atualizada</returns>
    /// <response code="200">Retorna a tarefa atualizada</response>
    /// <response code="400">Se os dados da tarefa forem inválidos</response>
    /// <response code="404">Se a tarefa não for encontrada</response>
    /// <response code="409">Se já existir uma tarefa com o mesmo título</response>
    /// <response code="500">Se ocorrer um erro interno</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequestDTO request)
    {
        _logger.LogInformation(string.Format(Messages.Log.Task.UpdatingTask, id));
        var result = await _updateTaskService.ExecuteAsync(id, request);
        return Ok(result);
    }

    /// <summary>
    /// Exclui uma tarefa
    /// </summary>
    /// <param name="id">ID da tarefa</param>
    /// <returns>Nenhum conteúdo</returns>
    /// <response code="204">Tarefa excluída com sucesso</response>
    /// <response code="404">Se a tarefa não for encontrada</response>
    /// <response code="500">Se ocorrer um erro interno</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation(string.Format(Messages.Log.Task.DeletingTask, id));
        await _deleteTaskService.ExecuteAsync(id);
        return NoContent();
    }
} 