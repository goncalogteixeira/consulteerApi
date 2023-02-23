using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Services;

namespace WebApiTemplate.Api.Application.Features.Todos.Queries
{
    public class GetTodoFromCacheQuery : IRequest<Result<TodoDTO>>
    {
        public int Id { get; set; }

        public GetTodoFromCacheQuery(int id) => Id = id;
    }

    public class GetTodoFromCacheQueryHandler : IRequestHandler<GetTodoFromCacheQuery, Result<TodoDTO>>
    {
        private readonly ITodoService _todoService;

        public GetTodoFromCacheQueryHandler(ITodoService todoService) => _todoService = todoService;

        public async Task<Result<TodoDTO>> Handle(GetTodoFromCacheQuery request, CancellationToken cancellationToken)
        {
            var todo = await _todoService.GetTodoAsync(request.Id);

            Result<TodoDTO> result;

            if (todo != null)
            {
                result = await Result<TodoDTO>.SuccessAsync(todo);
            }
            else
            {
                result = await Result<TodoDTO>.FailAsync(ApplicationConstants.ErrorFetchingDb);
            }

            return result;
        }
    }
}
