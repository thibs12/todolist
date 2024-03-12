using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly ITodoItemService _todoItermService;
    public TodoItemsController(ITodoItemService todoItemService)
    {
        _todoItermService = todoItemService;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        var res = await _todoItermService.GetTodoItems();
        return Ok(res);
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _todoItermService.GetTodoItem(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return todoItem;
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItemDTO>> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var res = await _todoItermService.UpdateTodoItem(id, todoDTO);

        if (res == null)
        {
            return NotFound();
        }

        return res;
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var res = await _todoItermService.CreateTodoItem(todoDTO);

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = res.Id },
                res);
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var res = await _todoItermService.DeleteTodoItem(id);
        if (!res)
        {
            return NotFound();
        }

        return NoContent();
    }
}