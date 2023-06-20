using Todo.Data.Models;

namespace Todo.Api.Handlers;

public record CompleteTodoItemRequest(Guid Id) : IRequest<bool>;

public class CompleteTodoItemHandler : IRequestHandler<CompleteTodoItemRequest, bool>
{
    private readonly ITodoRepository _todoRepository;

    public CompleteTodoItemHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<bool> Handle(CompleteTodoItemRequest request, CancellationToken cancellationToken)
    {
        var item = new TodoItem
        {
            Completed = DateTimeOffset.UtcNow,
            Id = request.Id
        };

        var success = await _todoRepository.Update(item);
        return success;
    }
}