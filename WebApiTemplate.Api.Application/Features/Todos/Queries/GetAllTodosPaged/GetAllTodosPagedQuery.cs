using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.DTOs.Common;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Features.Todos.Queries.GetAllTodosPaged
{
    public class GetAllTodosPagedQuery : IRequest<PagedResponse<TodoDTO>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public GetAllTodosPagedQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllTodosPagedQueryHandler : IRequestHandler<GetAllTodosPagedQuery, PagedResponse<TodoDTO>>
    {
        private readonly IBaseRepository<Todo> _repository;

        public GetAllTodosPagedQueryHandler(IUnitOfWork unitOfWork) => _repository = unitOfWork.GetRepository<Todo>();

        public async Task<PagedResponse<TodoDTO>> Handle(GetAllTodosPagedQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _repository.Entities;

            var resources = await _repository.GetPagedProjectionWithQueryAsync<TodoDTO>(
                request.PageNumber
                , request.PageSize
                , baseQuery
                );

            if (resources.Data != null)
            {
                return new PagedResponse<TodoDTO>(resources.Data.ToList(), request.PageNumber, request.PageSize, resources.Count);
            }
            else
            {
                return new PagedResponse<TodoDTO>(ApplicationConstants.ErrorFetchingDb);
            }
        }
    }
}
