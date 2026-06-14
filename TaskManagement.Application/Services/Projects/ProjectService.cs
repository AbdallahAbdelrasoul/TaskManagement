using TaskManagement.Application.Services.Context;
using TaskManagement.Application.Services.Projects.DTOs;
using TaskManagement.Application.Services.Tasks.DTOs;
using TaskManagement.Domain.Projects;
using TaskManagement.Domain.Shared.Exceptions;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Shared.Repositories;

namespace TaskManagement.Application.Services.Projects;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _uow;
    private readonly IActiveUserContext _userContext;

    public ProjectService(IUnitOfWork uow, IActiveUserContext userContext)
    {
        _uow = uow;
        _userContext = userContext;
    }

    public async Task<PaginatedResult<ProjectDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var result = await _uow.Projects.GetAllAsync(pagination, ct);
        var dtos = result.Items.Select(MapToDto).ToList();
        return new PaginatedResult<ProjectDto>(dtos, result.TotalCount, result.PageNumber, result.PageSize);
    }

    public async Task<ProjectDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var project = await _uow.Projects.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Project), id);

        return MapToDto(project);
    }

    public async Task<ProjectDto> CreateAsync(ProjectCreateDto request, int ownerId, CancellationToken ct = default)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _userContext.UserId
        };

        await _uow.Projects.AddAsync(project, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToDto(project);
    }

    public async Task<ProjectDto> UpdateAsync(int id, ProjectUpdateDto request, CancellationToken ct = default)
    {
        var project = await _uow.Projects.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Project), id);

        project.Name = request.Name;
        project.Description = request.Description;
        project.ModifiedBy = _userContext.UserId;

        await _uow.Projects.UpdateAsync(project, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToDto(project);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var project = await _uow.Projects.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Project), id);

        await _uow.Projects.DeleteAsync(project, ct);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task<PaginatedResult<TaskDto>> GetProjectTasksAsync(int projectId, PaginationParams pagination, CancellationToken ct = default)
    {
        _ = await _uow.Projects.GetByIdAsync(projectId, ct)
            ?? throw new NotFoundException(nameof(Project), projectId);

        var result = await _uow.Projects.GetProjectTasksAsync(projectId, pagination, ct);
        var dtos = result.Items.Select(t => new TaskDto(
            t.Id, t.Title, t.Description,
            t.Status.ToString(), t.Priority.ToString(),
            t.DueDate, t.ProjectId, t.CreatedOn)).ToList();

        return new PaginatedResult<TaskDto>(dtos, result.TotalCount, result.PageNumber, result.PageSize);
    }

    private static ProjectDto MapToDto(Project p) =>
        new(p.Id, p.Name, p.Description, p.OwnerId, p.CreatedAt, p.CreatedOn);
}
