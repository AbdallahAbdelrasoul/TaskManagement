using TaskManagement.DataAccess;
using TaskManagement.Domain.Shared.Repositories;

namespace TaskManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public IProjectRepository Projects { get; }

    public UnitOfWork(AppDbContext context, IUserRepository users, IProjectRepository projects)
    {
        _context = context;
        Users = users;
        Projects = projects;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
