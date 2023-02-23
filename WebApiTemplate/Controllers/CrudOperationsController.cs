using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Results;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud;
using WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.CreateTodoDatabase;
using WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.UpdateBatchTodo;
using WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.UpdateTodoDatabase;
using WebApiTemplate.Api.Application.Features.Todos.Queries;
using WebApiTemplate.Api.Application.Features.Todos.Queries.GetAllTodosPaged;

namespace WebApiTemplate.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CrudOperationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CrudOperationsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            GetAllTodosFromDbQuery query = new();

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

        [HttpGet]
        public async Task<IActionResult> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var todos = await _mediator.Send(new GetAllTodosPagedQuery(pageNumber, pageSize));

            IActionResult actionResult;

            if (todos.Data != null)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFromCacheAsync()
        {
            var todos = await _mediator.Send(new GetAllFromCacheQuery());

            IActionResult actionResult;

            if (todos.Data != null)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailFromCacheAsync(int id)
        {
            var todo = await _mediator.Send(new GetTodoFromCacheQuery(id));

            IActionResult actionResult;

            if (todo.Succeeded)
            {
                actionResult = Ok(todo);
            }
            else
            {
                actionResult = Problem(todo.Message);
            }

            return actionResult;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync(CreateTodoDatabaseCommand command)
        {
            var todos = await _mediator.Send(command);

            IActionResult actionResult;

            if (todos.Succeeded == true)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTodoAsync(UpdateTodoDatabaseCommand command)
        {
            var todos = await _mediator.Send(command);

            IActionResult actionResult;

            if (todos.Succeeded == true)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTodoBatchAsync(UpdateBatchTodoCommand command)
        {
            var todos = await _mediator.Send(command);

            IActionResult actionResult;

            if (todos.Succeeded == true)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodoAsync(DeleteTodoCommand command)
        {
            var todos = await _mediator.Send(command);

            IActionResult actionResult;

            if (todos.Succeeded == true)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBatchTodoAsync(DeleteBatchDatabaseCommand command)
        {
            var todos = await _mediator.Send(command);

            IActionResult actionResult;

            if (todos.Succeeded == true)
            {
                actionResult = Ok(todos);
            }
            else
            {
                actionResult = Problem(todos.Message);
            }

            return actionResult;
        }
    }
}
