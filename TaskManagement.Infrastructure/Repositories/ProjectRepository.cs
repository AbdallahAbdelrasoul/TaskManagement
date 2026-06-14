using Microsoft.EntityFrameworkCore;
using TaskManagement.DataAccess;
using TaskManagement.Domain.Projects;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Shared.Repositories;

namespace TaskManagement.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context) => _context = context;

    public async Task<Project?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Projects
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<PaginatedResult<Project>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var query = _context.Projects.Include(p => p.Owner).AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return new PaginatedResult<Project>(items, total, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken ct = default)
    {
        await _context.Projects.AddAsync(project, ct);
        return project;
    }

    public Task UpdateAsync(Project project, CancellationToken ct = default)
    {
        _context.Projects.Update(project);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Project project, CancellationToken ct = default)
    {
        _context.Projects.Remove(project);
        return Task.CompletedTask;
    }
}
