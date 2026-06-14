using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services.Auth;
using TaskManagement.Application.Services.Auth.DTOs;
using TaskManagement.Domain.Shared.Models;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request, CancellationToken ct)
    {
        var result = await _authService.RegisterAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<AuthDto>.Ok(result, "User registered successfully."));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request, ct);
        return Ok(ApiResponse<AuthDto>.Ok(result, "Login successful."));
    }
}
