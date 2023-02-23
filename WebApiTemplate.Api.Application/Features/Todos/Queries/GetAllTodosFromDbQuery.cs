using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.DTOs.Results;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Features.Todos.Queries
{

    public class GetAllTodosFromDbQuery : IRequest<ListResult<TodoDTO>>
    {

    }

    public class GetAllTododsDbQueryHandler : IRequestHandler<GetAllTodosFromDbQuery, ListResult<TodoDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllTododsDbQueryHandler(
            IUnitOfWork unitOfWork
            , IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ListResult<TodoDTO>> Handle(GetAllTodosFromDbQuery request, CancellationToken cancellationToken)
        {
            List<Todo> todosEntity = await _unitOfWork.GetRepository<Todo>().GetAllAsync();
            var todos = _mapper.Map<List<TodoDTO>>(todosEntity);

            ListResult<TodoDTO> result;

            if (todos != null)
            {
                result = ListResult<TodoDTO>.Success(todos);
            }
            else
            {
                result = ListResult<TodoDTO>.Failure(ApplicationConstants.ErrorFetchingDb);
            }

            return result;
        }
    }
}
