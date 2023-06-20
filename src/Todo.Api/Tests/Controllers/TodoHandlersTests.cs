using Moq;
using NUnit.Framework;
using Todo.Api.Handlers;
using Todo.Data.Models;

namespace Todo.Api.Tests.Handlers
{
    [TestFixture]
    public class TodoHandlersTests
    {
        private Mock<ITodoRepository> _repositoryMock;
        private ListTodoItemsHandler _listTodoItemsHandler;
        private CreateTodoItemHandler _createTodoItemHandler;
        private CompleteTodoItemHandler _completeTodoItemHandler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _listTodoItemsHandler = new ListTodoItemsHandler(_repositoryMock.Object);
            _createTodoItemHandler = new CreateTodoItemHandler(_repositoryMock.Object);
            _completeTodoItemHandler = new CompleteTodoItemHandler(_repositoryMock.Object);
        }

        [Test]
        public async Task ListTodoItemsHandler_HideCompletedTrue_ReturnsFilteredTodoItems()
        {
            // Arrange
            var request = new ListTodoItemsRequest(HideCompleted: true);
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 1", Created = DateTimeOffset.UtcNow, Completed = DateTimeOffset.UtcNow },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 2", Created = DateTimeOffset.UtcNow },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 3", Created = DateTimeOffset.UtcNow, Completed = DateTimeOffset.UtcNow },
            };
            _repositoryMock.Setup(x => x.List()).ReturnsAsync(todoItems);

            // Act
            var result = await _listTodoItemsHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Text, Is.EqualTo("Task 2"));
        }

        [Test]
        public async Task ListTodoItemsHandler_HideCompletedTrue_ReturnsAllTodoItems()
        {
            // Arrange
            var request = new ListTodoItemsRequest(HideCompleted: false);
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 1", Created = DateTimeOffset.UtcNow, Completed = DateTimeOffset.UtcNow },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 2", Created = DateTimeOffset.UtcNow },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 3", Created = DateTimeOffset.UtcNow, Completed = DateTimeOffset.UtcNow },
            };
            _repositoryMock.Setup(x => x.List()).ReturnsAsync(todoItems);

            // Act
            var result = await _listTodoItemsHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task CreateTodoItemHandler_ValidRequest_CreatesTodoItem()
        {
            // Arrange
            var request = new CreateTodoItemRequest("New Task");
            var createdItemId = Guid.NewGuid();
            _repositoryMock.Setup(x => x.Create(It.IsAny<TodoItem>())).ReturnsAsync(createdItemId);

            // Act
            var result = await _createTodoItemHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(createdItemId));
            _repositoryMock.Verify(x => x.Create(It.IsAny<TodoItem>()), Times.Once);
        }

        [Test]
        public void CreateTodoItemHandler_EmptyText_ThrowsArgumentException()
        {
            // Arrange
            var request = new CreateTodoItemRequest(string.Empty);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _createTodoItemHandler.Handle(request, CancellationToken.None));
            _repositoryMock.Verify(x => x.Create(It.IsAny<TodoItem>()), Times.Never);
        }

        [Test]
        public async Task CompleteTodoItemHandler_ValidRequest_UpdatesTodoItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var request = new CompleteTodoItemRequest(itemId);
            _repositoryMock.Setup(x => x.Update(It.IsAny<TodoItem>())).ReturnsAsync(true);

            // Act
            var result = await _completeTodoItemHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.True);
            _repositoryMock.Verify(x => x.Update(It.IsAny<TodoItem>()), Times.Once);
        }
    }
}
