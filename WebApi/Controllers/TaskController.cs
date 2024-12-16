using Application.Common.SQL;
using Application.CQRS.Tasks.TaskQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly SqlManager _sqlManager;
        private readonly IMediator _mediator;

        public TaskController(SqlManager sqlManager,
                              IMediator mediator)
        {
            _sqlManager = sqlManager;
            _mediator = mediator;
        }

        [HttpGet("get-task")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery { Id = taskId });

            if (task == null)
                return NotFound($"Задача с ID {taskId} не найдена.");

            task.QueryData = await _sqlManager.ExecuteQueryAsync(task.Decision);

            return Ok(task);
        }
    }
}
