namespace Invoicing.Receivables.Domain.Exceptions;

public class InputException : ArgumentException
{
    public InputException(string? message)
        : base(message)
    {
    }

    public InputException(string? paramName, string? message)
        : base(message, paramName)
    {
    }
}
