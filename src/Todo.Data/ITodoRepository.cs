using Todo.Data.Models;

namespace Todo.Data;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> List();
    
    Task<Guid> Create(TodoItem newItem);

    /// <summary>
    /// Updates a TodoItem in the database.
    /// </summary>
    /// <param name="updatedItem">The updated TodoItem object.</param>
    /// <returns>A task representing the asynchronous operation with a boolean indicating the success of the update operation.</returns>
    Task<bool> Update(TodoItem updatedItem);
}