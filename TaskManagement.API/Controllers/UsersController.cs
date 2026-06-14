using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services.Users;
using TaskManagement.Application.Services.Users.DTOs;
using TaskManagement.Domain.Shared.Models;
using TaskManagement.Domain.Shared.Pagination;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<UserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination, CancellationToken ct)
    {
        var result = await _userService.GetAllAsync(pagination, ct);
        return Ok(ApiResponse<PaginatedResult<UserDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _userService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<UserDto>.Ok(result));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto request, CancellationToken ct)
    {
        var result = await _userService.UpdateAsync(id, request, ct);
        return Ok(ApiResponse<UserDto>.Ok(result, "User updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _userService.DeleteAsync(id, ct);
        return Ok(ApiResponse<object>.Ok(null!, "User deleted successfully."));
    }
}
