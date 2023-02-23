using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Results;
using WebApiTemplate.Api.Application.DTOs.Settings;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Services;

namespace WebApiTemplate.Api.Application.Features.Todos.Queries
{
    public class GetAllTodosQuery : IRequest<ListResult<TodoDTO>>
    {

    }

    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, ListResult<TodoDTO>>
    {
        private readonly IExternalTodosService _externalTodosService;
        private readonly ExternalApiSettings _externalApiSettings;

        public GetAllTodosQueryHandler(
            IExternalTodosService externalTodosService
            , IOptions<ExternalApiSettings> externalApiSettings
            )
        {
            _externalTodosService = externalTodosService;
            _externalApiSettings = externalApiSettings.Value;
        }

        public async Task<ListResult<TodoDTO>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            List<TodoDTO> todos = await _externalTodosService.GetTodosAsync();

            ListResult<TodoDTO> result;

            if (todos != null)
            {
                result = ListResult<TodoDTO>.Success(todos);
            }
            else
            {
                result = ListResult<TodoDTO>.Failure(_externalApiSettings.FailureMessage);
            }

            return result;
        }
    }
}
