using Application;
using Application.Common.SQL;
using Application.Common.SQL.ResponseModels;
using Application.CQRS.Tasks.TaskQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/sql-query")]
    [ApiController]
    public class SqlQueryController : ControllerBase
    {
        private readonly SqlManager _sqlManager;
        private readonly IMediator _mediator;

        public SqlQueryController(IMediator mediator, SqlManager sqlManager)
        {
            _sqlManager = sqlManager;
            _mediator = mediator;
        }

        [HttpPost("validate-sql")]
        public async Task<IActionResult> ValidateSqlSyntax([FromBody] SqlRequestModel request)
        {
            var response = await _sqlManager.ValidateSqlSyntaxAsync(request.SqlQuery);

            if (response == null)
                return BadRequest("Не удалось обработать запрос.");

            return Ok(response);
        }


        [HttpPost("compare-sql")]
        public async Task<IActionResult> CompareSql([FromBody] CompareSqlRequestModel request)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery { Id = request.TaskId });

            if (task == null)
                return NotFound($"Задача с ID {request.TaskId} не найдена.");

            var response = await _sqlManager.CompareSqlResultsAsync(task.Decision, request.SqlQuery);

            if (response == null)
                return BadRequest("Ошибка сравнения данных SQL.");

            return Ok(response);
        }



    }

    public class SqlRequestModel
    {
        public string SqlQuery { get; set; }
    }

    public class CompareSqlRequestModel
    {
        public int TaskId { get; set; }
        public string SqlQuery { get; set; }
    }

}
