using consulteer.Application.TodoLists.Queries.ExportTodos;

namespace consulteer.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
