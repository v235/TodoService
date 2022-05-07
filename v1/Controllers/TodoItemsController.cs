using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Contract;
using TodoApiDTO.Infrasctructure.CustomException;
using TodoApiDTO.v1.Resources;

namespace TodoApiDTO.v1.Controllers
{
    /// <summary>
    /// TodoItemsController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        /// <summary>
        /// The todo items recource
        /// </summary>
        private readonly ITodoItemsRecource _todoItemsRecource;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoItemsController"/> class.
        /// </summary>
        /// <param name="todoItemsRecource">The todo items recource.</param>
        /// <exception cref="ArgumentNullException">todoItemsRecource</exception>
        public TodoItemsController(ITodoItemsRecource todoItemsRecource)
        {
            _todoItemsRecource = todoItemsRecource ?? throw new ArgumentNullException(nameof(todoItemsRecource));
        }

        /// <summary>
        /// Gets the todo items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var result = await _todoItemsRecource.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Gets the todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            try
            {
                var result = await _todoItemsRecource.GetByIdAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates the todo item.
        /// </summary>
        /// <param name="todoItemDTO">The todo item dto.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem([FromBody] TodoItemDTO todoItemDTO)
        {
            try
            {
                var result = await _todoItemsRecource.CreateAsync(todoItemDTO);

                return CreatedAtAction(nameof(GetTodoItem), new { id = result.Id }, result);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates the todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="todoItemDTO">The todo item dto.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, [FromBody] TodoItemDTO todoItemDTO)
        {
            try
            {
                var result = await _todoItemsRecource.UpdateAsync(id, todoItemDTO);

                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes the todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            try
            {
                await _todoItemsRecource.DeleteAsync(id);

                return NoContent();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }
    }
}