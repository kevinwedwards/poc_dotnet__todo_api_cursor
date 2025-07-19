using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Controller for managing todo items
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        /// <summary>
        /// Gets all todo items ordered by their order field
        /// </summary>
        /// <returns>A list of all todo items</returns>
        /// <response code="200">Returns the list of todo items</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var todos = await _todoService.GetAllTodosAsync();
            return Ok(todos);
        }

        /// <summary>
        /// Gets all todo items created by a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of todo items created by the user</returns>
        /// <response code="200">Returns the list of todo items</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodosByUser(int userId)
        {
            var todos = await _todoService.GetTodosByUserIdAsync(userId);
            return Ok(todos);
        }

        /// <summary>
        /// Gets a specific todo item by its ID
        /// </summary>
        /// <param name="id">The ID of the todo item</param>
        /// <returns>The todo item if found</returns>
        /// <response code="200">Returns the requested todo item</response>
        /// <response code="404">If the todo item is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            
            if (todo == null)
            {
                return NotFound($"Todo with ID {id} not found");
            }

            return Ok(todo);
        }

        /// <summary>
        /// Gets all overdue todo items
        /// </summary>
        /// <returns>A list of overdue todo items</returns>
        /// <response code="200">Returns the list of overdue todo items</response>
        [HttpGet("overdue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Todo>>> GetOverdueTodos()
        {
            var todos = await _todoService.GetOverdueTodosAsync();
            return Ok(todos);
        }

        /// <summary>
        /// Gets todo items within a date range
        /// </summary>
        /// <param name="startDate">Start date for the range</param>
        /// <param name="endDate">End date for the range</param>
        /// <returns>A list of todo items within the date range</returns>
        /// <response code="200">Returns the list of todo items</response>
        [HttpGet("daterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodosByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var todos = await _todoService.GetTodosByDateRangeAsync(startDate, endDate);
            return Ok(todos);
        }

        /// <summary>
        /// Creates a new todo item
        /// </summary>
        /// <param name="createTodoDto">The todo item to create</param>
        /// <returns>The newly created todo item</returns>
        /// <response code="201">Returns the newly created todo item</response>
        /// <response code="400">If the todo item data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Todo>> CreateTodo(CreateTodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _todoService.CreateTodoAsync(createTodoDto);
            
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        /// <summary>
        /// Updates an existing todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to update</param>
        /// <param name="updateTodoDto">The updated todo item data</param>
        /// <returns>The updated todo item</returns>
        /// <response code="200">Returns the updated todo item</response>
        /// <response code="400">If the todo item data is invalid</response>
        /// <response code="404">If the todo item is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, UpdateTodoDto updateTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _todoService.UpdateTodoAsync(id, updateTodoDto);
            
            if (todo == null)
            {
                return NotFound($"Todo with ID {id} not found");
            }

            return Ok(todo);
        }

        /// <summary>
        /// Deletes a todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the todo item was successfully deleted</response>
        /// <response code="404">If the todo item is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var deleted = await _todoService.DeleteTodoAsync(id);
            
            if (!deleted)
            {
                return NotFound($"Todo with ID {id} not found");
            }

            return NoContent();
        }
    }
} 