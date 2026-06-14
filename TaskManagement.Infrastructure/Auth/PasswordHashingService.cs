using TaskManagement.Application.Services.Auth;

namespace TaskManagement.Infrastructure.Auth;

public class PasswordHashingService : IPasswordHashingService
{
    public string Hash(string plainPassword) =>
        BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: 12);

    public bool Verify(string plainPassword, string hashedPassword) =>
        BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
}
