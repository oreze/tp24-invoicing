namespace Identity.Receivables.ApplicationContracts.DTOs;

public class DebtorDTO
{
    public int ID { get; set; }
    public string Reference { get; set; }
    public string Name { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Town { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string CountryCode { get; set; }
    public string? RegistrationNumber { get; set; }
}