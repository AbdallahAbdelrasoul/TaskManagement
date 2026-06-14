namespace TaskManagement.Domain.Shared.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException(string entityName, string fieldName, object value)
        : base($"{entityName} with {fieldName} '{value}' already exists.")
    {
    }
}
