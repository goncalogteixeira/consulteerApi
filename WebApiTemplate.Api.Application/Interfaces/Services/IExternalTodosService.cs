using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Todos;

namespace WebApiTemplate.Api.Application.Interfaces.Services
{
    public interface IExternalTodosService
    {
        Task<List<TodoDTO>> GetTodosAsync();

        Task<TodoDTO> CreateTodoAsync(TodoCreateDTO todo);
    }
}
