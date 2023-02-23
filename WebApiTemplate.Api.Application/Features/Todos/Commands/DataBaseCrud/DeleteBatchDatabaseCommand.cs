using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud
{
    public partial class DeleteBatchDatabaseCommand : IRequest<Result<List<TodoDTO>>>
    {
        public List<TodoDTO> Items { get; }

        public DeleteBatchDatabaseCommand(List<TodoDTO> items) => Items = items;
    }

    public class DeleteBatchDatabaseCommandHandler : IRequestHandler<DeleteBatchDatabaseCommand, Result<List<TodoDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public DeleteBatchDatabaseCommandHandler(
            IUnitOfWork unitOfWork
            , IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<TodoDTO>>> Handle(DeleteBatchDatabaseCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetRepository<Todo>().BulkDeleteAsync(_mapper.Map<List<Todo>>(request.Items));

            await _unitOfWork.CommitAsync(cancellationToken);

            Result<List<TodoDTO>> result;

            result = await Result<List<TodoDTO>>.SuccessAsync(data: request.Items);

            return result;
        }
    }
}
