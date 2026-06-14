namespace TaskManagement.Application.Services.Auth.DTOs;

public record AuthDto(int UserId, string Username, string Email, string Role, string Token, DateTime ExpiresAt);
