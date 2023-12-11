using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.UnitTests.Domain.Entities;

public class CurrencyTests
{
    [Theory]
    [InlineData("USD", "US Dollar")]
    [InlineData("PLN", "Polish Zloty")]
    [InlineData("EUR", "Euro")]
    public void Create_ValidInput_ReturnsCurrencyInstance(string code, string name)
    {
        var currency = Currency.Create(code, name);

        Assert.NotNull(currency);
        Assert.Equal(code, currency.Code);
        Assert.Equal(name, currency.Name);
    }

    [Theory]
    [InlineData("", "Name")]
    [InlineData("Code", null)]
    [InlineData(null, "Name")]
    [InlineData("Code", "")]
    [InlineData(null, null)]
    [InlineData("EURO", "Euro")]
    public void Create_InvalidInput_ThrowsException(string code, string name)
    {
        // Arrange, Act & Assert
        Assert.ThrowsAny<InputException>(() => Currency.Create(code, name));
    }
}
