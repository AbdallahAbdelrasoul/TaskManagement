using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Tasks;
using DomainTaskStatus = TaskManagement.Domain.Tasks.TaskStatus;

namespace TaskManagement.Domain.Shared.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PaginatedResult<TaskItem>> GetAllAsync(
        PaginationParams pagination,
        int? projectId = null,
        DomainTaskStatus? status = null,
        CancellationToken ct = default);
    Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct = default);
    Task UpdateAsync(TaskItem task, CancellationToken ct = default);
    Task DeleteAsync(TaskItem task, CancellationToken ct = default);
}
