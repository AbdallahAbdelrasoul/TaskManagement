namespace TaskManagement.Application.Services.Tasks.DTOs;

public record TaskUpdateDto(string Title, string? Description, string Status, string Priority, DateTime? DueDate);
