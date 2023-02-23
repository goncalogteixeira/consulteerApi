using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Constants;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.UpdateTodoDatabase
{
    public partial class UpdateTodoDatabaseCommand : IRequest<Result<TodoDTO>>
    {
        public int Id { get; }

        public int UserId { get; }

        public string Title { get; }

        public string Body { get; }

        public UpdateTodoDatabaseCommand(
            int id
            , int userId
            , string title
            , string body
            )
        {
            Id = id;
            UserId = userId;
            Title = title;
            Body = body;
        }
    }

    public class UpdateTodoDatabaseCommandHandler : IRequestHandler<UpdateTodoDatabaseCommand, Result<TodoDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public UpdateTodoDatabaseCommandHandler(
            IUnitOfWork unitOfWork
            , IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TodoDTO>> Handle(UpdateTodoDatabaseCommand request, CancellationToken cancellationToken)
        {
            Result<TodoDTO> result;

            Todo entity = await _unitOfWork.GetRepository<Todo>().GetByIdAsync(request.Id);

            if (entity != null)
            {
                entity.UserId = request.UserId;
                entity.Title = request.Title;
                entity.Body = request.Body;

                _unitOfWork.GetRepository<Todo>().Update(entity);

                await _unitOfWork.CommitAsync(cancellationToken);

                var dto = _mapper.Map<TodoDTO>(entity);

                result = await Result<TodoDTO>.SuccessAsync(dto);
            }
            else
            {
                result = await Result<TodoDTO>.FailAsync(ApplicationConstants.ErrorUpdatingDb);
            }

            return result;
        }
    }
}
