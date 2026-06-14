using TaskManagement.Application.Services.Projects.DTOs;
using TaskManagement.Application.Services.Tasks.DTOs;
using TaskManagement.Domain.Shared.Pagination;

namespace TaskManagement.Application.Services.Projects;

public interface IProjectService
{
    Task<PaginatedResult<ProjectDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<ProjectDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProjectDto> CreateAsync(ProjectCreateDto request, int ownerId, CancellationToken ct = default);
    Task<ProjectDto> UpdateAsync(int id, ProjectUpdateDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<PaginatedResult<TaskDto>> GetProjectTasksAsync(int projectId, PaginationParams pagination, CancellationToken ct = default);
}
