using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Controllers;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;


namespace TaskManagerTests
{
    public class PendingTaskControllerTests
    {
        private readonly TaskDbContext _context;
        private readonly PendingTaskController _controller;

        public PendingTaskControllerTests()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TaskDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();


            _controller = new PendingTaskController(_context);

            if (!_context.Tasks.Any())
            {
                _context.Tasks.AddRange(
                    new PendingTask { Title = "Task 1", Description = "Test Task 1" }, // Elimina la asignación manual de IDs
                    new PendingTask { Title = "Task 2", Description = "Test Task 2" }
                );
                _context.SaveChanges();
            }
        }

       [Fact]
        public async Task GetPendingTask_ReturnsTask_WhenIdIsValid()
        {
            // Act
            var result = await _controller.GetPendingTask(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PendingTask>>(result);
            var task = Assert.IsType<PendingTask>(actionResult.Value);
            Assert.Equal(1, task.ID);
        }

        [Fact]
        public async Task PostPendingTask_CreatesTask()
        {
            // Arrange
            var newTask = new PendingTask { Title = "New Task", Description = "New Task Description" };

            // Act
            var result = await _controller.PostPendingTask(newTask);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PendingTask>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var task = Assert.IsType<PendingTask>(createdResult.Value);
            Assert.Equal("New Task", task.Title);
        }

    }
}
