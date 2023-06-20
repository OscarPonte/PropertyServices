using Microsoft.AspNetCore.Mvc;
using Todo.Api.Handlers;

namespace Todo.Api.Controllers;

[ApiController]
[Route("todo")]
public class TodoController : ControllerBase
{
    private readonly ISender _sender;

    public TodoController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(bool hideCompleted = false) =>
        Ok(await _sender.Send(new ListTodoItemsRequest(hideCompleted)));
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemRequest request) =>
        Ok(await _sender.Send(request));

    [HttpPost("Complete")]
    public async Task<IActionResult> Complete([FromBody] CompleteTodoItemRequest request) =>
        await _sender.Send(request) ? Ok() : BadRequest();
}