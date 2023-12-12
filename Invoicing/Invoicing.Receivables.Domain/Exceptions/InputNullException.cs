namespace Invoicing.Receivables.Domain.Exceptions;

public class InputNullException : InputException
{
    public InputNullException(string? paramName)
        : base(paramName)
    {
    }

    public InputNullException(string? paramName, string? message)
        : base(paramName, message)
    {
    }
}