using FluentValidation;
using TaskManagement.Application.Services.Users.DTOs;

namespace TaskManagement.Application.Services.Users.Validators;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Username must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");
    }
}
