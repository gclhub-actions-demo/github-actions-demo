using DotnetApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Map health check endpoint
app.MapHealthChecks("/health");

// Map controllers
app.MapControllers();

// In-memory task storage
var tasks = new List<TaskItem>
{
    new TaskItem { Id = 1, Title = "Complete project", Description = "Finish the demo project", IsCompleted = false },
    new TaskItem { Id = 2, Title = "Write tests", Description = "Add unit tests", IsCompleted = true },
    new TaskItem { Id = 3, Title = "Deploy", Description = "Deploy to production", IsCompleted = false }
};
var nextId = 4;

// Minimal API endpoints for tasks
app.MapGet("/api/tasks", () => Results.Ok(new { tasks }));

app.MapGet("/api/tasks/{id:int}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    return task is not null ? Results.Ok(task) : Results.NotFound(new { error = "Task not found" });
});

app.MapPost("/api/tasks", ([FromBody] TaskItem task) =>
{
    if (string.IsNullOrEmpty(task.Title))
    {
        return Results.BadRequest(new { error = "Title is required" });
    }

    var newTask = new TaskItem
    {
        Id = nextId++,
        Title = task.Title,
        Description = task.Description,
        IsCompleted = task.IsCompleted
    };

    tasks.Add(newTask);
    return Results.Created($"/api/tasks/{newTask.Id}", newTask);
});

app.MapPut("/api/tasks/{id:int}", (int id, [FromBody] TaskItem updatedTask) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null)
    {
        return Results.NotFound(new { error = "Task not found" });
    }

    task.Title = updatedTask.Title ?? task.Title;
    task.Description = updatedTask.Description ?? task.Description;
    task.IsCompleted = updatedTask.IsCompleted;

    return Results.Ok(task);
});

app.MapDelete("/api/tasks/{id:int}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null)
    {
        return Results.NotFound(new { error = "Task not found" });
    }

    tasks.Remove(task);
    return Results.NoContent();
});

app.Run();

// Make Program class accessible for testing
public partial class Program { }
