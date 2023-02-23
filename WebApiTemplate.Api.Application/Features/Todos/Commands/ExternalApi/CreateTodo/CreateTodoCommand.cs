using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Settings;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Services;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.ExternalApi.CreateTodo
{
    public partial class CreateTodoCommand : IRequest<Result<TodoDTO>>
    {
        public int UserId { get; }

        public string Title { get; }

        public string Body { get; }

        public CreateTodoCommand(
            int userId
            , string title
            , string body
            )
        {
            UserId = userId;
            Title = title;
            Body = body;
        }
    }

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Result<TodoDTO>>
    {

        private readonly IExternalTodosService _externalTodosService;
        private readonly ExternalApiSettings _externalApiSettings;
        private readonly IMapper _mapper;

        public CreateTodoCommandHandler(
            IExternalTodosService externalTodosService
            , IOptions<ExternalApiSettings> externalApiSettings
            , IMapper mapper)
        {
            _externalTodosService = externalTodosService;
            _externalApiSettings = externalApiSettings.Value;
            _mapper = mapper;
        }

        public async Task<Result<TodoDTO>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            TodoCreateDTO todo = _mapper.Map<TodoCreateDTO>(request);

            TodoDTO createdTodo = await _externalTodosService.CreateTodoAsync(todo);

            Result<TodoDTO> result;

            if (createdTodo != null)
            {
                result = await Result<TodoDTO>.SuccessAsync(createdTodo);
            }
            else
            {
                result = await Result<TodoDTO>.FailAsync(_externalApiSettings.FailureMessage);
            }

            return result;
        }
    }
}
