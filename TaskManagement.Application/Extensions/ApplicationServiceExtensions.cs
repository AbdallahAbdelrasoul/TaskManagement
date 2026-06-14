using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Services.Auth;
using TaskManagement.Application.Services.Auth.Validators;
using TaskManagement.Application.Services.Projects;
using TaskManagement.Application.Services.Users;
using TaskManagement.Domain.Shared.Validation;

namespace TaskManagement.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IValidationEngine, ValidationEngine>();

        services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();

        return services;
    }
}
