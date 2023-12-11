using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Infrastructure.Configuration.EntitiesConfiguration;
using Invoicing.Receivables.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Invoicing.Receivables.UnitTests.Infrastructure.Configuration.EntitiesConfiguration;

public class InvoiceTypeEntityConfigurationTests
{
    [Fact]
    public void Configure_InvoiceEntity_ConfiguresProperties()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("test_database")
            .Options;

        // Arrange
        using var dbContext = new AppDbContext(options);
        var builder = new ModelBuilder(new ConventionSet());
        var configuration = new InvoiceTypeEntityConfiguration();

        // Act
        configuration.Configure(builder.Entity<Invoice>());

        // Assert
        var entityType = dbContext.Model.FindEntityType(typeof(Invoice));
        Assert.NotNull(entityType);

        Assert.Equal(nameof(Invoice.ID), entityType.FindPrimaryKey().Properties.Single().Name);

        Assert.Equal(512, entityType.FindProperty(nameof(Invoice.Reference)).GetMaxLength());
        Assert.False(entityType.FindProperty(nameof(Invoice.Reference)).IsNullable);

        Assert.False(entityType.FindProperty(nameof(Invoice.IssueDate)).IsNullable);
        Assert.False(entityType.FindProperty(nameof(Invoice.OpeningValue)).IsNullable);
        Assert.False(entityType.FindProperty(nameof(Invoice.PaidValue)).IsNullable);
        Assert.False(entityType.FindProperty(nameof(Invoice.DueDate)).IsNullable);

        Assert.True(entityType.FindProperty(nameof(Invoice.ClosedDate)).IsNullable);
        Assert.True(entityType.FindProperty(nameof(Invoice.Cancelled)).IsNullable);

        var referenceProperty = entityType.FindProperty(nameof(Invoice.Reference));

        Assert.NotNull(referenceProperty);

        var indexOnReference = entityType.GetIndexes()
            .FirstOrDefault(index => index.Properties.Contains(referenceProperty));

        Assert.NotNull(indexOnReference);
        Assert.True(indexOnReference.IsUnique);


        var debtorNavigation = entityType.FindNavigation(nameof(Invoice.Debtor));
        Assert.NotNull(debtorNavigation);
        Assert.Equal(nameof(Invoice.DebtorID), debtorNavigation.ForeignKey.Properties.Single().Name);

        var currencyNavigation = entityType.FindNavigation(nameof(Invoice.Currency));
        Assert.NotNull(currencyNavigation);
        Assert.Equal(nameof(Invoice.CurrencyCode), currencyNavigation.ForeignKey.Properties.Single().Name);
    }
}