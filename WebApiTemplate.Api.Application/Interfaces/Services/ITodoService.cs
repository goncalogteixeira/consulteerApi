using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Todos;

namespace WebApiTemplate.Api.Application.Interfaces.Services
{

    public interface ITodoService
    {
        Task<TodoDTO> GetTodoAsync(int id);

        Task<List<TodoDTO>> GetAllTodosAsync();

        Task FlushCache();
    }
}
