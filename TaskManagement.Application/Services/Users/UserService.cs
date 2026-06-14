using TaskManagement.Application.Services.Users.DTOs;
using TaskManagement.Domain.Shared.Exceptions;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Shared.Repositories;
using TaskManagement.Domain.Users;

namespace TaskManagement.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;

    public UserService(IUnitOfWork uow) => _uow = uow;

    public async Task<PaginatedResult<UserDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var result = await _uow.Users.GetAllAsync(pagination, ct);
        var dtos = result.Items.Select(MapToDto).ToList();
        return new PaginatedResult<UserDto>(dtos, result.TotalCount, result.PageNumber, result.PageSize);
    }

    public async Task<UserDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(User), id);

        return MapToDto(user);
    }

    public async Task<UserDto> UpdateAsync(int id, UserUpdateDto request, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(User), id);

        user.Username = request.Username;
        user.Email = request.Email;

        await _uow.Users.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToDto(user);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(User), id);

        await _uow.Users.DeleteAsync(user, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private static UserDto MapToDto(User user) =>
        new(user.Id, user.Username, user.Email, user.Role.ToString(), user.CreatedOn);
}
