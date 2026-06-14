using TaskManagement.Application.Services.Context;
using TaskManagement.Application.Services.Tasks.DTOs;
using TaskManagement.Domain.Shared.Exceptions;
using TaskManagement.Domain.Shared.Pagination;
using TaskManagement.Domain.Shared.Repositories;
using TaskManagement.Domain.Tasks;
using TaskStatus = TaskManagement.Domain.Tasks.TaskStatus;

namespace TaskManagement.Application.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _uow;
    private readonly IActiveUserContext _userContext;

    public TaskService(IUnitOfWork uow, IActiveUserContext userContext)
    {
        _uow = uow;
        _userContext = userContext;
    }

    public async Task<PaginatedResult<TaskDto>> GetAllAsync(PaginationParams pagination, int? projectId, string? status, CancellationToken ct = default)
    {
        TaskStatus? taskStatus = null;
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TaskStatus>(status, true, out var parsed))
            taskStatus = parsed;

        var result = await _uow.Tasks.GetAllAsync(pagination, projectId, taskStatus, ct);
        var dtos = result.Items.Select(MapToDto).ToList();
        return new PaginatedResult<TaskDto>(dtos, result.TotalCount, result.PageNumber, result.PageSize);
    }

    public async Task<TaskDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var task = await _uow.Tasks.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(TaskItem), id);

        return MapToDto(task);
    }

    public async Task<TaskDto> CreateAsync(TaskCreateDto request, CancellationToken ct = default)
    {
        var priority = Enum.Parse<TaskPriority>(request.Priority, true);

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            Status = TaskStatus.Todo,
            CreatedBy = _userContext.UserId
        };

        await _uow.Tasks.AddAsync(task, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToDto(task);
    }

    public async Task<TaskDto> UpdateAsync(int id, TaskUpdateDto request, CancellationToken ct = default)
    {
        var task = await _uow.Tasks.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(TaskItem), id);

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = Enum.Parse<TaskStatus>(request.Status, true);
        task.Priority = Enum.Parse<TaskPriority>(request.Priority, true);
        task.DueDate = request.DueDate;
        task.ModifiedBy = _userContext.UserId;

        await _uow.Tasks.UpdateAsync(task, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToDto(task);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var task = await _uow.Tasks.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(TaskItem), id);

        await _uow.Tasks.DeleteAsync(task, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private static TaskDto MapToDto(TaskItem t) =>
        new(t.Id, t.Title, t.Description, t.Status.ToString(), t.Priority.ToString(), t.DueDate, t.ProjectId, t.CreatedOn);
}
