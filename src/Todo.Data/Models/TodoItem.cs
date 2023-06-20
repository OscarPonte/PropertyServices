namespace Todo.Data.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    
    public DateTimeOffset Created { get; set; }
    
    public string? Text { get; set; }
    
    public DateTimeOffset? Completed { get; set; }
}