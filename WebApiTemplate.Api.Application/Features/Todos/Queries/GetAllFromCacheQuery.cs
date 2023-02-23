using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.DTOs.Results;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Services;

namespace WebApiTemplate.Api.Application.Features.Todos.Queries
{
    public class GetAllFromCacheQuery : IRequest<ListResult<TodoDTO>>
    {

    }

    public class GetAllFromCacheQueryHandler : IRequestHandler<GetAllFromCacheQuery, ListResult<TodoDTO>>
    {
        private readonly ITodoService _todoService;

        public GetAllFromCacheQueryHandler(ITodoService todoService) => _todoService = todoService;

        public async Task<ListResult<TodoDTO>> Handle(GetAllFromCacheQuery request, CancellationToken cancellationToken)
        {
            List<TodoDTO> todos = await _todoService.GetAllTodosAsync();

            if (todos != null)
            {
                return ListResult<TodoDTO>.Success(todos);
            }
            else
            {
                return ListResult<TodoDTO>.Failure(ApplicationConstants.ErrorFetchingDb);
            }
        }
    }
}
