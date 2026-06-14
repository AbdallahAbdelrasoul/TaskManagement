using TaskManagement.DataAccess;
using TaskManagement.Domain.Shared.Repositories;

namespace TaskManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }

    public UnitOfWork(AppDbContext context, IUserRepository users)
    {
        _context = context;
        Users = users;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
