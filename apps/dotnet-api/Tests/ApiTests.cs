using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using DotnetApi.Models;
using Xunit;

namespace DotnetApi.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllTasks_ReturnsTaskList()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TasksResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Tasks);
        Assert.NotEmpty(result.Tasks);
    }

    [Fact]
    public async Task GetTask_WithValidId_ReturnsTask()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var task = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.NotNull(task);
        Assert.Equal(1, task.Id);
        Assert.NotNull(task.Title);
    }

    [Fact]
    public async Task GetTask_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateTask_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newTask = new TaskItem
        {
            Title = "New Task",
            Description = "Test task",
            IsCompleted = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", newTask);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdTask = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.NotNull(createdTask);
        Assert.True(createdTask.Id > 0);
        Assert.Equal(newTask.Title, createdTask.Title);
        Assert.Equal(newTask.Description, createdTask.Description);
    }

    [Fact]
    public async Task CreateTask_WithMissingTitle_ReturnsBadRequest()
    {
        // Arrange
        var invalidTask = new TaskItem
        {
            Description = "No title",
            IsCompleted = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", invalidTask);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_WithValidData_ReturnsUpdatedTask()
    {
        // Arrange
        var updatedTask = new TaskItem
        {
            Title = "Updated Title",
            Description = "Updated Description",
            IsCompleted = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/tasks/1", updatedTask);

        // Assert
        response.EnsureSuccessStatusCode();
        var task = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.NotNull(task);
        Assert.Equal(updatedTask.Title, task.Title);
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public async Task UpdateTask_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var updatedTask = new TaskItem
        {
            Title = "Updated Title"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/tasks/999", updatedTask);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_WithValidId_ReturnsNoContent()
    {
        // First, create a task to delete
        var newTask = new TaskItem { Title = "To Delete", Description = "Will be deleted" };
        var createResponse = await _client.PostAsJsonAsync("/api/tasks", newTask);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskItem>();
        Assert.NotNull(createdTask);

        // Act
        var response = await _client.DeleteAsync($"/api/tasks/{createdTask.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/tasks/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private class TasksResponse
    {
        public List<TaskItem>? Tasks { get; set; }
    }
}
