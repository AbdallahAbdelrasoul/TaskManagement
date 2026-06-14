namespace TaskManagement.Application.Services.Auth;

public interface IPasswordHashingService
{
    string Hash(string plainPassword);
    bool Verify(string plainPassword, string hashedPassword);
}
