namespace TaskManagement.Application.Services.Projects.DTOs;

public record ProjectDto(int Id, string Name, string? Description, int OwnerId, DateTime CreatedAt, DateTime? CreatedOn);
