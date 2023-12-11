namespace Identity.Receivables.ApplicationContracts.DTOs.Statistics;

public class TotalRevenuePerCurrencyDTO
{
    public IEnumerable<MoneyDTO> Values { get; set; }
}