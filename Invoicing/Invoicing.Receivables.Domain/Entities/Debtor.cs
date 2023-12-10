using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.Domain.Entities;

public class Debtor
{
    private Debtor()
    {
    }

    public int ID { get; set; }
    public string Reference { get; private set; }
    public string Name { get; private set; }
    public string? Address1 { get; private set; }
    public string? Address2 { get; private set; }
    public string? Town { get; private set; }
    public string? State { get; private set; }
    public string? Zip { get; private set; }
    public string CountryCode { get; private set; }
    public string? RegistrationNumber { get; private set; }

    public static Debtor Create(string reference, string name, string? address1, string? address2, string? town, string? state, string? zip, string countryCode, string? registrationNumber)
    {
        ValidateInput(reference, name, countryCode);

        return new Debtor
        {
            Reference = reference,
            Name = name,
            Address1 = address1,
            Address2 = address2,
            Town = town,
            State = state,
            Zip = zip,
            CountryCode = countryCode,
            RegistrationNumber = registrationNumber
        };
    }

    public static void ValidateInput(string reference, string name, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new InputNullException(nameof(reference), "Debtor reference number cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(name))
            throw new InputNullException(nameof(name), "Debtor name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(reference))
            throw new InputNullException(nameof(countryCode), "Debtor country code cannot be null or empty.");
    }
}