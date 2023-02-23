using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Results;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Features.Todos.Commands.ExternalApi.CreateTodo;
using WebApiTemplate.Api.Application.Features.Todos.Queries;

namespace WebApiTemplate.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExternalAPIController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExternalAPIController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            GetAllTodosQuery query = new();

            ListResult<TodoDTO> todos = await _mediator.Send(query);

            IActionResult actionResult;

            if (todos.Succeeded)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateTodoCommand todo)
        {
            Result<TodoDTO> createdTodo = await _mediator.Send(todo);

            IActionResult actionResult;

            if (createdTodo.Succeeded)
            {
                actionResult = CreatedAtAction(null, createdTodo);
            }
            else
            {
                actionResult = Problem(createdTodo.Message);
            }

            return actionResult;
        }
    }
}
