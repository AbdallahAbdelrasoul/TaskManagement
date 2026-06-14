using TaskManagement.Domain.Projects;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Tasks;

namespace TaskManagement.Domain.Shared.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PaginatedResult<Project>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<PaginatedResult<TaskItem>> GetProjectTasksAsync(int projectId, PaginationParams pagination, CancellationToken ct = default);
    Task<Project> AddAsync(Project project, CancellationToken ct = default);
    Task UpdateAsync(Project project, CancellationToken ct = default);
    Task DeleteAsync(Project project, CancellationToken ct = default);
}
