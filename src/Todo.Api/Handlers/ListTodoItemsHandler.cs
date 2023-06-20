using Todo.Data.Models;

namespace Todo.Api.Handlers;

public record ListTodoItemsRequest(bool HideCompleted) : IRequest<IEnumerable<TodoItem>>;

public class ListTodoItemsHandler : IRequestHandler<ListTodoItemsRequest, IEnumerable<TodoItem>>
{
    private readonly ITodoRepository _todoRepository;

    public ListTodoItemsHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<TodoItem>> Handle(ListTodoItemsRequest request, CancellationToken cancellationToken)
    {
        var todoItems = await _todoRepository.List();

        if (request.HideCompleted)
        {
            todoItems = todoItems.Where(x => !x.Completed.HasValue);
        }

        return todoItems.OrderByDescending(x => x.Created);
    }
}