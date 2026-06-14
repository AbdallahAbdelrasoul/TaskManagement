namespace TaskManagement.Application.Services.Auth.DTOs;

public record UserRegisterDto(string Username, string Email, string Password, string ConfirmPassword);
