namespace TaskManagement.Domain.Shared.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IProjectRepository Projects { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
