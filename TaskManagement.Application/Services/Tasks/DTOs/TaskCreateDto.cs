namespace TaskManagement.Application.Services.Tasks.DTOs;

public record TaskCreateDto(string Title, string? Description, string Priority, DateTime? DueDate, int ProjectId);
