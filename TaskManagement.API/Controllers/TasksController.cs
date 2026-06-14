using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services.Tasks;
using TaskManagement.Application.Services.Tasks.DTOs;
using TaskManagement.Domain.Shared.Models;
using TaskManagement.Domain.Shared.Pagination;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService) => _taskService = taskService;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationParams pagination,
        [FromQuery] int? projectId,
        [FromQuery] string? status,
        CancellationToken ct)
    {
        var result = await _taskService.GetAllAsync(pagination, projectId, status, ct);
        return Ok(ApiResponse<PaginatedResult<TaskDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _taskService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<TaskDto>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] TaskCreateDto request, CancellationToken ct)
    {
        var result = await _taskService.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<TaskDto>.Ok(result, "Task created successfully."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto request, CancellationToken ct)
    {
        var result = await _taskService.UpdateAsync(id, request, ct);
        return Ok(ApiResponse<TaskDto>.Ok(result, "Task updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _taskService.DeleteAsync(id, ct);
        return Ok(ApiResponse<object>.Ok(null!, "Task deleted successfully."));
    }
}
