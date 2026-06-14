using TaskManagement.Application.Services.Projects.DTOs;
using TaskManagement.Domain.Projects;
using TaskManagement.Domain.Shared.Exceptions;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Shared.Repositories;

namespace TaskManagement.Application.Services.Projects;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _uow;

    public ProjectService(IUnitOfWork uow) => _uow = uow;

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
            CreatedAt = DateTime.UtcNow
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

    private static ProjectDto MapToDto(Project p) =>
        new(p.Id, p.Name, p.Description, p.OwnerId, p.CreatedAt, p.CreatedOn);
}
