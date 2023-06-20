using Todo.Data.Models;

namespace Todo.Data;

public interface ITodoRepository
{
    /// <summary>
    /// Retrieves a list of todo items.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and contains the list of todo items.</returns>
    Task<IEnumerable<TodoItem>> List();

    /// <summary>
    /// Creates a new todo item.
    /// </summary>
    /// <param name="newItem">The todo item to create.</param>
    /// <returns>A task that represents the asynchronous operation and contains the ID of the newly created todo item.</returns>
    Task<Guid> Create(TodoItem newItem);

    /// <summary>
    /// Updates a TodoItem in the database.
    /// </summary>
    /// <param name="updatedItem">The updated TodoItem object.</param>
    /// <returns>A task representing the asynchronous operation with a boolean indicating the success of the update operation.</returns>
    Task<bool> Update(TodoItem updatedItem);
}