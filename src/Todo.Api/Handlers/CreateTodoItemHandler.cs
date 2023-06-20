using Todo.Data.Models;

namespace Todo.Api.Handlers;

public record CreateTodoItemRequest(string Text) : IRequest<Guid>;

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemRequest, Guid>
{
    private readonly ITodoRepository _todoRepository;

    public CreateTodoItemHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Guid> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Text))
        {
            throw new ArgumentException("Cannot create an empty Todo item. Please provide a valid item");
        }

        var item = new TodoItem
        {
            Created = DateTimeOffset.UtcNow,
            Id = Guid.NewGuid(),
            Text = request.Text
        };

        var itemId = await _todoRepository.Create(item);
        return itemId;
    }
}