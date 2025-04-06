using Microsoft.AspNetCore.Mvc;

namespace LooveTaskManager.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;

    public TaskController(ILogger<TaskController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetTasks")]
    public IEnumerable<TaskController> Get()
    {
        return new List<TaskController>();
    }
}
