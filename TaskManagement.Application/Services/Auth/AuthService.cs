using TaskManagement.Application.Services.Auth.DTOs;
using TaskManagement.Domain.Shared.Repositories;
using TaskManagement.Domain.Users;
using DomainDuplicateException = TaskManagement.Domain.Shared.Exceptions.DuplicateException;
using DomainNotFoundException = TaskManagement.Domain.Shared.Exceptions.NotFoundException;
using DomainUnauthorizedException = TaskManagement.Domain.Shared.Exceptions.UnauthorizedException;

namespace TaskManagement.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHashingService _passwordHashingService;

    public AuthService(
        IUnitOfWork uow,
        IJwtTokenService jwtTokenService,
        IPasswordHashingService passwordHashingService)
    {
        _uow = uow;
        _jwtTokenService = jwtTokenService;
        _passwordHashingService = passwordHashingService;
    }

    public async Task<AuthDto> RegisterAsync(UserRegisterDto request, CancellationToken ct = default)
    {
        if (await _uow.Users.ExistsByEmailAsync(request.Email, ct))
            throw new DomainDuplicateException(nameof(User), nameof(request.Email), request.Email);

        if (await _uow.Users.ExistsByUsernameAsync(request.Username, ct))
            throw new DomainDuplicateException(nameof(User), nameof(request.Username), request.Username);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHashingService.Hash(request.Password),
            Role = UserRole.User
        };

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        var (token, expiresAt) = _jwtTokenService.GenerateToken(user);

        return new AuthDto(user.Id, user.Username, user.Email, user.Role.ToString(), token, expiresAt);
    }

    public async Task<AuthDto> LoginAsync(UserLoginDto request, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByEmailAsync(request.Email, ct)
            ?? throw new DomainNotFoundException(nameof(User), request.Email);

        if (!_passwordHashingService.Verify(request.Password, user.PasswordHash))
            throw new DomainUnauthorizedException("Invalid email or password.");

        var (token, expiresAt) = _jwtTokenService.GenerateToken(user);

        return new AuthDto(user.Id, user.Username, user.Email, user.Role.ToString(), token, expiresAt);
    }
}
