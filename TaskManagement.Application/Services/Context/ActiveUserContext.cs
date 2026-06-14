using System.Threading;

namespace TaskManagement.Application.Services.Context;

public class ActiveUserContext : IActiveUserContext
{
    private static readonly AsyncLocal<string?> _userId = new();
    private static readonly AsyncLocal<string?> _username = new();
    private static readonly AsyncLocal<string?> _email = new();
    private static readonly AsyncLocal<string?> _role = new();

    public string? UserId => _userId.Value;
    public string? Username => _username.Value;
    public string? Email => _email.Value;
    public string? Role => _role.Value;

    public void Set(string? userId, string? username, string? email, string? role)
    {
        _userId.Value = userId;
        _username.Value = username;
        _email.Value = email;
        _role.Value = role;
    }
}
