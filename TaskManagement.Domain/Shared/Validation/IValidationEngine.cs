using FluentValidation.Results;

namespace TaskManagement.Domain.Shared.Validation;

public interface IValidationEngine
{
    List<ValidationFailure>? Validate<T>(IValidationModel<T>? input, bool throwException = true);
}
