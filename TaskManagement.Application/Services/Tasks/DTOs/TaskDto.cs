namespace TaskManagement.Application.Services.Tasks.DTOs;

public record TaskDto(int Id, string Title, string? Description, string Status, string Priority, DateTime? DueDate, int ProjectId, DateTime? CreatedOn);
