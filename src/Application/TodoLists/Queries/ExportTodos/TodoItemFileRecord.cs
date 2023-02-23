using consulteer.Application.Common.Mappings;
using consulteer.Domain.Entities;

namespace consulteer.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
