namespace TaskManagement.Application.Services.Context
{
    public interface IActiveUserContext
    {
        string? UserId { get; }
        string? Username { get; }
        string? Email { get; }
        string? Role { get; }
        void Set(string? userId, string? username, string? email, string? role);
    }
}
