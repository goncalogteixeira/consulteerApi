using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.UpdateBatchTodo
{
    public partial class UpdateBatchTodoCommand : IRequest<Result<List<TodoDTO>>>
    {
        public List<TodoDTO> Items { get; }

        public UpdateBatchTodoCommand(List<TodoDTO> items) => Items = items;
    }

    public class UpdateBatchTodoCommandHandler : IRequestHandler<UpdateBatchTodoCommand, Result<List<TodoDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public UpdateBatchTodoCommandHandler(
            IUnitOfWork unitOfWork
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<TodoDTO>>> Handle(UpdateBatchTodoCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetRepository<Todo>().BulkUpdateAsync(_mapper.Map<List<Todo>>(request.Items));

            await _unitOfWork.CommitAsync(cancellationToken);

            return await Result<List<TodoDTO>>.SuccessAsync(data: request.Items);
        }
    }
}
