using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services.Projects;
using TaskManagement.Application.Services.Projects.DTOs;
using TaskManagement.Domain.Shared.Models;
using TaskManagement.Domain.Shared.Pagination;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService) => _projectService = projectService;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination, CancellationToken ct)
    {
        var result = await _projectService.GetAllAsync(pagination, ct);
        return Ok(ApiResponse<PaginatedResult<ProjectDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _projectService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<ProjectDto>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] ProjectCreateDto request, CancellationToken ct)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _projectService.CreateAsync(request, ownerId, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<ProjectDto>.Ok(result, "Project created successfully."));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto request, CancellationToken ct)
    {
        var result = await _projectService.UpdateAsync(id, request, ct);
        return Ok(ApiResponse<ProjectDto>.Ok(result, "Project updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _projectService.DeleteAsync(id, ct);
        return Ok(ApiResponse<object>.Ok(null!, "Project deleted successfully."));
    }
}
