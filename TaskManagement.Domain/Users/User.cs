using TaskManagement.Domain.Shared.Aggregates;

namespace TaskManagement.Domain.Users;

public class User : BaseDomain, IEntity<int>
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; } = UserRole.User;
}
