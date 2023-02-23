using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud
{
    public partial class DeleteTodoCommand : IRequest<Result<TodoDTO>>
    {
        public int Id { get; }

        public DeleteTodoCommand(int id) => Id = id;
    }

    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, Result<TodoDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public DeleteTodoCommandHandler(
            IUnitOfWork unitOfWork
            , IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TodoDTO>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            Todo todo = await _unitOfWork.GetRepository<Todo>().GetByIdAsync(request.Id);

            _unitOfWork.GetRepository<Todo>().Delete(todo);
            await _unitOfWork.CommitAsync(cancellationToken);
            var dto = _mapper.Map<TodoDTO>(todo);
            Result<TodoDTO> result;

            if (dto != null)
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
