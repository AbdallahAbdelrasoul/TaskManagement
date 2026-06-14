using Microsoft.EntityFrameworkCore;
using TaskManagement.DataAccess;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Shared.Repositories;
using TaskManagement.Domain.Users;

namespace TaskManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task<PaginatedResult<User>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var query = _context.Users.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(u => u.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return new PaginatedResult<User>(items, total, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<User> AddAsync(User user, CancellationToken ct = default)
    {
        await _context.Users.AddAsync(user, ct);
        return user;
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
        await _context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct = default) =>
        await _context.Users.AnyAsync(u => u.Username == username, ct);
}
