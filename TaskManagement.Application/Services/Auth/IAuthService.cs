using TaskManagement.Application.Services.Auth.DTOs;

namespace TaskManagement.Application.Services.Auth;

public interface IAuthService
{
    Task<AuthDto> RegisterAsync(UserRegisterDto request, CancellationToken ct = default);
    Task<AuthDto> LoginAsync(UserLoginDto request, CancellationToken ct = default);
}
