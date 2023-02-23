using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.CreateTodoDatabase
{

    public partial class CreateTodoDatabaseCommand : IRequest<Result<TodoDTO>>
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public CreateTodoDatabaseCommand(
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

    public class CreateTodoDatabaseCommandHandler : IRequestHandler<CreateTodoDatabaseCommand, Result<TodoDTO>>
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public CreateTodoDatabaseCommandHandler(
            IUnitOfWork unitOfWork
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TodoDTO>> Handle(CreateTodoDatabaseCommand request, CancellationToken cancellationToken)
        {
            TodoCreateDTO todo = _mapper.Map<TodoCreateDTO>(request);
            var entity = _mapper.Map<Todo>(todo);
            Todo createdTodo = _unitOfWork.GetRepository<Todo>().Add(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var dto = _mapper.Map<TodoDTO>(createdTodo);
            Result<TodoDTO> result;

            if (createdTodo != null)
            {
                result = await Result<TodoDTO>.SuccessAsync(dto);
            }
            else
            {
                result = await Result<TodoDTO>.FailAsync(ApplicationConstants.ErrorAddingDb);
            }

            return result;
        }
    }
}
