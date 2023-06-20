using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Todo.Api.Controllers;
using Todo.Api.Handlers;
using Todo.Data.Models;

namespace Todo.Api.Tests.Controllers
{
    [TestFixture]
    public class TodoControllerTests
    {
        private Mock<ISender> _senderMock;
        private TodoController _todoController;

        [SetUp]
        public void Setup()
        {
            _senderMock = new Mock<ISender>();
            _todoController = new TodoController(_senderMock.Object);
        }

        [Test]
        public async Task List_HideCompletedFalse_ReturnsAllTodoItems()
        {
            // Arrange
            var hideCompleted = false;
            var expectedRequest = new ListTodoItemsRequest(hideCompleted);
            var expectedResponse = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 1", Created = DateTimeOffset.UtcNow, Completed = DateTimeOffset.UtcNow },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 2", Created = DateTimeOffset.UtcNow },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 3", Created = DateTimeOffset.UtcNow, Completed = DateTimeOffset.UtcNow }
            };
            _senderMock.Setup(x => x.Send(expectedRequest, CancellationToken.None)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _todoController.List(hideCompleted);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var responseTodoItems = okResult.Value as IEnumerable<TodoItem>;
            Assert.NotNull(responseTodoItems);
            Assert.That(responseTodoItems.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task List_HideCompletedTrue_ReturnsFilteredTodoItems()
        {
            // Arrange
            var hideCompleted = false;
            var expectedRequest = new ListTodoItemsRequest(hideCompleted);
            var expectedResponse = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 1" },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 2" },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 3" }
            };
            _senderMock.Setup(x => x.Send(expectedRequest, CancellationToken.None)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _todoController.List(hideCompleted);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var responseTodoItems = okResult.Value as IEnumerable<TodoItem>;
            Assert.NotNull(responseTodoItems);
            Assert.That(responseTodoItems.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Create_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var expectedRequest = new CreateTodoItemRequest("New Task");
            var expectedResponse = Guid.NewGuid();
            _senderMock.Setup(x => x.Send(expectedRequest, CancellationToken.None)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _todoController.Create(expectedRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task Complete_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var expectedRequest = new CompleteTodoItemRequest(Guid.NewGuid());
            _senderMock.Setup(x => x.Send(expectedRequest, CancellationToken.None)).ReturnsAsync(true);

            // Act
            var result = await _todoController.Complete(expectedRequest);

            // Assert
            var okResult = result as OkResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Complete_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var expectedRequest = new CompleteTodoItemRequest(Guid.NewGuid());
            _senderMock.Setup(x => x.Send(expectedRequest, CancellationToken.None)).ReturnsAsync(false);

            // Act
            var result = await _todoController.Complete(expectedRequest);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }
    }
}