using Invoicing.Receivables.Domain.Exceptions;
using Invoicing.Receivables.Domain.ValueObjects;
using Invoicing.Receivables.Domain.ValueObjects.Statistics;

namespace Invoicing.Receivables.UnitTests.Domain.ValueObjects.Statistics;

public class TotalRevenuePerCurrencyTests
{
    [Fact]
    public void Constructor_ValidValues_CreatesInstance()
    {
        // Arrange
        var moneyList = new List<Money>
        {
            new(100.0m, "USD"),
            new(50.0m, "EUR")
        };

        // Act
        var totalRevenuePerCurrency = new TotalRevenuePerCurrency(moneyList);

        // Assert
        Assert.Equal(moneyList, totalRevenuePerCurrency.Values);
    }

    [Fact]
    public void Constructor_NullValues_ThrowsInputNullException()
    {
        // Act and Assert
        Assert.Throws<InputNullException>(() => new TotalRevenuePerCurrency(null));
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
        Assert.Throws<InputNullException>(() => new TotalRevenuePerCurrency(moneyListWithNullItem));
    }
}
