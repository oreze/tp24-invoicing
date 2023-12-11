using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.Domain.Entities;

public class Currency
{
    private Currency()
    {
    }

    public string Code { get; private set; }
    public string Name { get; private set; }

    public static Currency Create(string code, string name)
    {
        ValidateInput(code, name);

        return new Currency
        {
            Name = name,
            Code = code
        };
    }

    private static void ValidateInput(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new InputNullException(nameof(code), "Currency code cannot be null or empty.");

        if (code.Length != 3)
            throw new InputNullException(nameof(code), "Currency code has to be 3 characters long.");

        if (string.IsNullOrWhiteSpace(name))
            throw new InputException(nameof(name), "Currency name cannot be null or empty.");
    }
}