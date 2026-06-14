using TaskManagement.Application.Services.Users.DTOs;
using TaskManagement.Domain.Shared.Pagination;

namespace TaskManagement.Application.Services.Users;

public interface IUserService
{
    Task<PaginatedResult<UserDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<UserDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<UserDto> UpdateAsync(int id, UserUpdateDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
