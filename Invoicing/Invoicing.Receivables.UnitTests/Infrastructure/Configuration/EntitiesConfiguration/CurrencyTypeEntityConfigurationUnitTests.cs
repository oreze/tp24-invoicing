using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Infrastructure.Configuration.EntitiesConfiguration;
using Invoicing.Receivables.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Invoicing.Receivables.UnitTests.Infrastructure.Configuration.EntitiesConfiguration;

public class CurrencyTypeEntityConfigurationUnitTests
{
    [Fact]
    public void Configure_CurrencyEntity_ConfiguresProperties()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;

        using var dbContext = new AppDbContext(options);
        var builder = new ModelBuilder(new ConventionSet());
        var configuration = new CurrencyTypeEntityConfiguration();

        // Act
        configuration.Configure(builder.Entity<Currency>());

        // Assert
        var entityType = dbContext.Model.FindEntityType(typeof(Currency));
        Assert.NotNull(entityType);

        var codeProperty = entityType.FindProperty(nameof(Currency.Code));
        Assert.NotNull(codeProperty);
        Assert.Equal(3, codeProperty.GetMaxLength());

        var nameProperty = entityType.FindProperty(nameof(Currency.Name));
        Assert.NotNull(nameProperty);
        Assert.Equal(128, nameProperty.GetMaxLength());
        Assert.False(nameProperty.IsNullable);
    }
}
