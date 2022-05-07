using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Contract;

namespace TodoApiDTO.v1.Resources
{
    /// <summary>
    /// ITodoItemsRecource
    /// </summary>
    public interface ITodoItemsRecource
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TodoItemDTO>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<TodoItemDTO> GetByIdAsync(long id);

        /// <summary>
        /// Creates the specified todo item dto.
        /// </summary>
        /// <param name="todoItemDTO">The todo item dto.</param>
        /// <returns></returns>
        Task<TodoItemDTO> CreateAsync(TodoItemDTO todoItemDTO);

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="todoItemDTO">The todo item dto.</param>
        /// <returns></returns>
        Task<TodoItemDTO> UpdateAsync(long id, TodoItemDTO todoItemDTO);

        /// <summary>
        /// Deletes the todo item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task DeleteAsync(long id);
    }
}