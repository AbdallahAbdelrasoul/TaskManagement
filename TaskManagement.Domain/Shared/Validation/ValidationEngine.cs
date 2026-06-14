using FluentValidation.Results;

namespace TaskManagement.Domain.Shared.Validation;

public class ValidationEngine : IValidationEngine
{
    public List<ValidationFailure>? Validate<T>(IValidationModel<T>? input, bool throwException = true)
    {
        if (input is null) return null;

        var failures = input.ValidationErrors;

        if (failures is { Count: > 0 } && throwException)
            throw new Exceptions.ValidationException(failures);

        return failures;
    }
}
