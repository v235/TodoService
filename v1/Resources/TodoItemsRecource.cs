using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Contract;
using TodoApi.Data;
using TodoApiDTO.Infrasctructure.CustomException;
using TodoApiDTO.v1.Converter;

namespace TodoApiDTO.v1.Resources
{
    /// <summary>
    /// TodoItemsRecource
    /// </summary>
    public class TodoItemsRecource : ITodoItemsRecource
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly TodoContext _context;
        /// <summary>
        /// The item converter
        /// </summary>
        private readonly IItemConverter _itemConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoItemsRecource"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="itemConverter">The item converter.</param>
        public TodoItemsRecource(TodoContext context, IItemConverter itemConverter)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _itemConverter = itemConverter ?? throw new ArgumentNullException(nameof(itemConverter));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TodoItemDTO>> GetAllAsync()
        {
            return await _context.TodoItems.Select(x => _itemConverter.ToContract(x)).ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task<TodoItemDTO> GetByIdAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return DoGetByIdAsync(id);
        }

        /// <summary>
        /// Creates the specified todo item dto.
        /// </summary>
        /// <param name="todoItemDTO">The todo item dto.</param>
        /// <returns></returns>
        public Task<TodoItemDTO> CreateAsync(TodoItemDTO todoItemDTO)
        {
            if (todoItemDTO == null)
                throw new ArgumentNullException(nameof(todoItemDTO));

            return DoCreateAsync(todoItemDTO);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="todoItemDTO">The todo item dto.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TodoItemDTO> UpdateAsync(long id, TodoItemDTO todoItemDTO)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (todoItemDTO == null)
                throw new ArgumentNullException(nameof(todoItemDTO));

            return DoUpdateAsync(id, todoItemDTO);
        }

        /// <summary>
        /// Deletes the todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task DeleteAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return DoDeleteAsync(id);
        }

        /// <summary>
        /// Does the get by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private async Task<TodoItemDTO> DoGetByIdAsync(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
                throw new ResourceNotFoundException();

            return _itemConverter.ToContract(todoItem);
        }

        private async Task<TodoItemDTO> DoCreateAsync(TodoItemDTO todoItemDTO)
        {
            var domain = _itemConverter.ToDomain(todoItemDTO);

            await _context.TodoItems.AddAsync(domain);
            await _context.SaveChangesAsync();

            return _itemConverter.ToContract(domain);
        }

        private async Task<TodoItemDTO> DoUpdateAsync(long id, TodoItemDTO todoItemDTO)
        {
            var domainTodoItem = await _context.TodoItems.FindAsync(id);
            if (domainTodoItem == null)
                throw new ResourceNotFoundException();

            var forUpdate = _itemConverter.UpdateDomain(domainTodoItem, todoItemDTO);

            _context.Update(forUpdate);
            await _context.SaveChangesAsync();

            return _itemConverter.ToContract(forUpdate);
        }

        private async Task DoDeleteAsync(long id)
        {
            var domainTodoItem = await _context.TodoItems.FindAsync(id);
            if (domainTodoItem == null)
                throw new ResourceNotFoundException();

            _context.Remove(domainTodoItem);
            await _context.SaveChangesAsync();
        }
    }
}