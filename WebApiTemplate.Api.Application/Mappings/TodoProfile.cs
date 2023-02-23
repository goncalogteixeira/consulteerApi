using AutoMapper;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Features.Todos.Commands.DataBaseCrud.CreateTodoDatabase;
using WebApiTemplate.Api.Application.Features.Todos.Commands.ExternalApi.CreateTodo;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Application.Mappings
{
    internal class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<CreateTodoCommand, TodoCreateDTO>();

            CreateMap<CreateTodoDatabaseCommand, TodoCreateDTO>();

            CreateMap<TodoCreateDTO, Todo>();

            CreateMap<Todo, TodoDTO>().ReverseMap();
        }
    }
}
