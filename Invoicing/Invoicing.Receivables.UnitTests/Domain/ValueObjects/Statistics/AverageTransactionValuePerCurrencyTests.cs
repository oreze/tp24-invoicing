using Invoicing.Receivables.Domain.Exceptions;
using Invoicing.Receivables.Domain.ValueObjects;
using Invoicing.Receivables.Domain.ValueObjects.Statistics;

namespace Invoicing.Receivables.UnitTests.Domain.ValueObjects.Statistics;

public class AverageTransactionValuePerCurrencyTests
{
    [Fact]
    public void AverageTransactionValuePerCurrency_Constructor_ValidValues_CreatesInstance()
    {
        // Arrange
        var moneyList = new List<Money>
        {
            new(100.0m, "USD"),
            new(50.0m, "EUR")
        };

        // Act
        var averageTransactionValuePerCurrency = new AverageTransactionValuePerCurrency(moneyList);

        // Assert
        Assert.Equal(moneyList, averageTransactionValuePerCurrency.Values);
    }

    [Fact]
    public void AverageTransactionValuePerCurrency_Constructor_NullValues_ThrowsInputNullException()
    {
        // Act and Assert
        Assert.Throws<InputNullException>(() => new AverageTransactionValuePerCurrency(null));
    }

    [Fact]
    public void Constructor_NullItemInList_ThrowsInputNullException()
    {
        // Arrange
        var moneyListWithNullItem = new List<Money>
        {
            new(100.0m, "USD"),
            null,
            new(50.0m, "EUR")
        };

        // Act and Assert
        Assert.Throws<InputNullException>(() => new AverageTransactionValuePerCurrency(moneyListWithNullItem));
    }
}