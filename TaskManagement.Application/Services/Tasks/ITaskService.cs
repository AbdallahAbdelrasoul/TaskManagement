using TaskManagement.Application.Services.Tasks.DTOs;
using TaskManagement.Domain.Shared.Pagination;

namespace TaskManagement.Application.Services.Tasks;

public interface ITaskService
{
    Task<PaginatedResult<TaskDto>> GetAllAsync(PaginationParams pagination, int? projectId, string? status, CancellationToken ct = default);
    Task<TaskDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TaskDto> CreateAsync(TaskCreateDto request, CancellationToken ct = default);
    Task<TaskDto> UpdateAsync(int id, TaskUpdateDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
