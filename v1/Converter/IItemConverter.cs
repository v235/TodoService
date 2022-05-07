using TodoApi.Contract;
using TodoApi.Domain;

namespace TodoApiDTO.v1.Converter
{
    /// <summary>
    /// IItemConverter
    /// </summary>
    public interface IItemConverter
    {
        /// <summary>
        /// Converts to contract.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TodoItemDTO ToContract(TodoItem source);

        /// <summary>
        /// Converts to domain.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TodoItem ToDomain(TodoItemDTO source);

        /// <summary>
        /// Updates domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TodoItem UpdateDomain(TodoItem domain, TodoItemDTO source);
    }
}