namespace TaskManagement.Domain.Shared.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
