using Microsoft.EntityFrameworkCore;
using TaskManagement.DataAccess;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Shared.Repositories;
using TaskManagement.Domain.Tasks;
using DomainTaskStatus = TaskManagement.Domain.Tasks.TaskStatus;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) => _context = context;

    public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<PaginatedResult<TaskItem>> GetAllAsync(
        PaginationParams pagination,
        int? projectId = null,
        DomainTaskStatus? status = null,
        CancellationToken ct = default)
    {
        var query = _context.Tasks.AsNoTracking();

        if (projectId.HasValue)
            query = query.Where(t => t.ProjectId == projectId.Value);

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(t => t.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return new PaginatedResult<TaskItem>(items, total, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct = default)
    {
        await _context.Tasks.AddAsync(task, ct);
        return task;
    }

    public Task UpdateAsync(TaskItem task, CancellationToken ct = default)
    {
        _context.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem task, CancellationToken ct = default)
    {
        _context.Tasks.Remove(task);
        return Task.CompletedTask;
    }
}
