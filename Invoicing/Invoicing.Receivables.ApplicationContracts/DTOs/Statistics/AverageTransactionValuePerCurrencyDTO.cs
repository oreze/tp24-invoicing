namespace Identity.Receivables.ApplicationContracts.DTOs.Statistics;

public class AverageTransactionValuePerCurrencyDTO
{
    public IEnumerable<MoneyDTO> Values { get; set; }
}