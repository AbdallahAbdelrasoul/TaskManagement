using TaskManagement.Domain.Shared.Aggregates;
using TaskManagement.Domain.Users;

namespace TaskManagement.Domain.Projects;

public class Project : BaseDomain, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int OwnerId { get; set; }

    public User Owner { get; set; } = default!;
}
