using TaskManagement.Domain.Users;

namespace TaskManagement.Application.Services.Auth;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
