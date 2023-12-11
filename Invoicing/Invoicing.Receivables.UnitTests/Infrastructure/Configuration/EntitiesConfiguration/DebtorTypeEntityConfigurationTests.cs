using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Infrastructure.Configuration.EntitiesConfiguration;
using Invoicing.Receivables.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Invoicing.Receivables.UnitTests.Infrastructure.Configuration.EntitiesConfiguration;

public class DebtorTypeEntityConfigurationTests
{
    [Fact]
    public void Configure_DebtorEntity_ConfiguresProperties()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;
        
        // Arrange
        using var dbContext = new AppDbContext(options);
        var builder = new ModelBuilder(new ConventionSet());
        var configuration = new DebtorTypeEntityConfiguration();

        // Act
        configuration.Configure(builder.Entity<Debtor>());

        // Assert
        var entityType = dbContext.Model.FindEntityType(typeof(Debtor));
        Assert.NotNull(entityType);

        Assert.Equal(nameof(Debtor.ID), entityType.FindPrimaryKey().Properties.Single().Name);

        Assert.Equal(255, entityType.FindProperty(nameof(Debtor.Reference)).GetMaxLength());
        Assert.False(entityType.FindProperty(nameof(Debtor.Reference)).IsNullable);

        Assert.Equal(255, entityType.FindProperty(nameof(Debtor.Name)).GetMaxLength());
        Assert.False(entityType.FindProperty(nameof(Debtor.Name)).IsNullable);

        Assert.Equal(255, entityType.FindProperty(nameof(Debtor.Address1)).GetMaxLength());
        Assert.Equal(255, entityType.FindProperty(nameof(Debtor.Address2)).GetMaxLength());
        Assert.Equal(255, entityType.FindProperty(nameof(Debtor.Town)).GetMaxLength());
        Assert.Equal(255, entityType.FindProperty(nameof(Debtor.State)).GetMaxLength());
        Assert.Equal(20, entityType.FindProperty(nameof(Debtor.Zip)).GetMaxLength());

        Assert.Equal(2, entityType.FindProperty(nameof(Debtor.CountryCode)).GetMaxLength());
        Assert.False(entityType.FindProperty(nameof(Debtor.CountryCode)).IsNullable);

        Assert.Equal(50, entityType.FindProperty(nameof(Debtor.RegistrationNumber)).GetMaxLength());

        var referenceProperty = entityType.FindProperty(nameof(Debtor.Reference));

        Assert.NotNull(referenceProperty);

        var indexOnReference = entityType.GetIndexes()
            .FirstOrDefault(index => index.Properties.Contains(referenceProperty));

        Assert.NotNull(indexOnReference);
        Assert.True(indexOnReference.IsUnique);    }
}
