namespace DotnetApi.Models;

/// <summary>
/// Represents a task item.
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Gets or sets the task ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the task title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the task description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether the task is completed.
    /// </summary>
    public bool IsCompleted { get; set; }
}
