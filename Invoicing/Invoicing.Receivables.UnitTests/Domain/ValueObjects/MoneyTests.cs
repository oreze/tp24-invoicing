using Invoicing.Receivables.Domain.Exceptions;
using Invoicing.Receivables.Domain.ValueObjects;

namespace Invoicing.Receivables.UnitTests.Domain.ValueObjects;

public class MoneyTests
{
    [Theory]
    [InlineData(100.0, "EUR")]
    [InlineData(-1, "USD")]
    public void Constructor_ValidParameters_CreatesInstance(decimal amount, string currencyCode)
    {
        // Act
        var money = new Money(amount, currencyCode);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currencyCode, money.CurrencyCode);
    }

    [Theory]
    [InlineData(100.0, "US")]
    [InlineData(100, "INVALID")]
    [InlineData(100, "")]
    [InlineData(100, null)]
    public void Constructor_InvalidParameters_ThrowsInputException(decimal amount, string currencyCode)
    {
        // Act and Assert
        Assert.Throws<InputException>(() => new Money(amount, currencyCode));
    }
}
