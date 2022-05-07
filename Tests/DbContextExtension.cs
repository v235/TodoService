using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoApi.Data;

namespace Tests
{
    internal static class DbContextExtension
    {
        public static TodoContext CreateContext<T>(this IEnumerable<T> data, string dbName = "MockedList")
            where T : class
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new TodoContext(options);
            context.AddRange(data);
            context.SaveChanges();

            return context;
        }
    }
}