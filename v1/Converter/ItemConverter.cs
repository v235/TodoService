using TodoApi.Contract;
using TodoApi.Domain;

namespace TodoApiDTO.v1.Converter
{
    /// <summary>
    /// ItemConverter
    /// </summary>
    public class ItemConverter : IItemConverter
    {
        /// <summary>
        /// Converts to contract.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public TodoItemDTO ToContract(TodoItem source)
        {
            if (source == null)
                return null;

            return new TodoItemDTO
            {
                Id = source.Id,
                Name = source.Name,
                IsComplete = source.IsComplete
            };
        }

        /// <summary>
        /// Converts to domain.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public TodoItem ToDomain(TodoItemDTO source)
        {
            if (source == null)
                return null;

            return new TodoItem
            {
                Name = source.Name,
                IsComplete = source.IsComplete
            };
        }

        /// <summary>
        /// Updates domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public TodoItem UpdateDomain(TodoItem domain, TodoItemDTO source)
        {
            if (domain == null)
                return null;

            if (source == null)
                return null;

            domain.Name = source.Name;
            domain.IsComplete = source.IsComplete;

            return domain;
        }
    }
}