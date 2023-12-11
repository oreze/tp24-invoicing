using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.UnitTests.Domain.Entities;

public class DebtorTests
{
    [Fact]
    public void Create_ValidInput_ReturnsDebtorInstance()
    {
        // Arrange
        var reference = "123";
        var name = "John Doe";
        var countryCode = "US";

        // Act
        var debtor = Debtor.Create(reference, name, null, null, null, null, null, countryCode, null);

        // Assert
        Assert.NotNull(debtor);
        Assert.Equal(reference, debtor.Reference);
        Assert.Equal(name, debtor.Name);
        Assert.Null(debtor.Address1);
        Assert.Null(debtor.Address2);
        Assert.Null(debtor.Town);
        Assert.Null(debtor.State);
        Assert.Null(debtor.Zip);
        Assert.Equal(countryCode, debtor.CountryCode);
        Assert.Null(debtor.RegistrationNumber);
    }

    [Theory]
    [InlineData(null, "John Doe", "US")]
    [InlineData("123", null, "US")]
    [InlineData("123", "John Doe", null)]
    public void Create_InvalidInput_ThrowsInputNullException(string reference, string name, string countryCode)
    {
        // Arrange, Act & Assert
        Assert.ThrowsAny<InputException>(() => Debtor.Create(reference, name, null, null, null, null, null, countryCode, null));
    }

    [Theory]
    [InlineData("USD", "US Dollar")]
    [InlineData("EUR", "Euro")]
    [InlineData("GBP", "British Pound")]
    public void Create_ValidInput_PropertiesAreSet(string code, string name)
    {
        // Arrange
        var debtor = Debtor.Create("123", "John Doe", null, null, null, null, null, code, null);

        // Act & Assert
        Assert.Equal("123", debtor.Reference);
        Assert.Equal("John Doe", debtor.Name);
        Assert.Null(debtor.Address1);
        Assert.Null(debtor.Address2);
        Assert.Null(debtor.Town);
        Assert.Null(debtor.State);
        Assert.Null(debtor.Zip);
        Assert.Equal(code, debtor.CountryCode);
        Assert.Null(debtor.RegistrationNumber);
    }
}