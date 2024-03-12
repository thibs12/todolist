using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItemDTO>> GetTodoItems();
        Task<TodoItemDTO> GetTodoItem(long id);
        Task<TodoItemDTO> CreateTodoItem(TodoItemDTO todoItemDTO);
        Task<TodoItemDTO> UpdateTodoItem(long id, TodoItemDTO todoItemDTO);
        Task<bool> DeleteTodoItem(long id);
        TodoItemDTO ItemToDTO(TodoItem todoItem);
        bool TodoItemExists(long id);
    }
}
