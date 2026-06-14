using TaskManagement.Domain.Projects;
using TaskManagement.Domain.Shared.Aggregates;

namespace TaskManagement.Domain.Tasks;

public class TaskItem : BaseDomain, IEntity<int>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }

    public Project Project { get; set; } = default!;
}
