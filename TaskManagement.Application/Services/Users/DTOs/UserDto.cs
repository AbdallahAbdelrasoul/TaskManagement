namespace TaskManagement.Application.Services.Users.DTOs;

public record UserDto(int Id, string Username, string Email, string Role, DateTime? CreatedOn);
