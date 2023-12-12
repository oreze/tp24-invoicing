using Identity.Receivables.ApplicationContracts.DTOs.Enums;
using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using Invoicing.Receivables.Domain.Enums;
using Invoicing.Receivables.Domain.ValueObjects;
using Invoicing.Receivables.Domain.ValueObjects.Statistics;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Statistics;
using Invoicing.Receivables.Infrastructure.Services;
using Invoicing.Receivables.UnitTests.Common.EqualityComparers;
using Moq;

namespace Invoicing.Receivables.UnitTests.Application.Services;

public class StatisticsServiceTests
{
    [Fact]
    public async Task GetTotalRevenuePerCurrencyAsync_ReturnsTotalRevenuePerCurrencyDTO()
    {
        // Arrange
        var statisticsRepositoryMock = new Mock<IStatisticsRepository>();
        var expectedTotalRevenue = new TotalRevenuePerCurrency(
            new List<Money> { new(100, "USD"), new(200, "EUR") });

        statisticsRepositoryMock.Setup(repo => repo.GetTotalRevenuePerCurrencyAsync())
            .ReturnsAsync(expectedTotalRevenue);

        var statisticsService = new StatisticsService(statisticsRepositoryMock.Object);

        // Act
        var result = await statisticsService.GetTotalRevenuePerCurrencyAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TotalRevenuePerCurrencyDTO>(result);

        var expectedMoneyDTOs = expectedTotalRevenue.Values
            .Select(item => new MoneyDTO { CurrencyCode = item.CurrencyCode, Amount = item.Amount });

        Assert.Equal(expectedMoneyDTOs, result.Values, new MoneyDTOEqualityComparer());
    }

    [Fact]
    public async Task GetAverageTransactionValuePerCurrencyAsync_ReturnsAverageTransactionValuePerCurrencyDTO()
    {
        // Arrange
        var statisticsRepositoryMock = new Mock<IStatisticsRepository>();
        var expectedAverageTransactionValue = new AverageTransactionValuePerCurrency(
            new List<Money> { new(50, "USD"), new(75, "EUR") });

        statisticsRepositoryMock.Setup(repo => repo.GetAverageTransactionValuePerCurrencyAsync())
            .ReturnsAsync(expectedAverageTransactionValue);

        var statisticsService = new StatisticsService(statisticsRepositoryMock.Object);

        // Act
        var result = await statisticsService.GetAverageTransactionValuePerCurrencyAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AverageTransactionValuePerCurrencyDTO>(result);

        var expectedMoneyDTOs = expectedAverageTransactionValue.Values
            .Select(item => new MoneyDTO { CurrencyCode = item.CurrencyCode, Amount = item.Amount });

        Assert.Equal(expectedMoneyDTOs, result.Values, new MoneyDTOEqualityComparer());
    }

    [Fact]
    public async Task GetPaymentStatusDistributionPerCurrencyAsync_ReturnsPaymentDistributionPerCurrencyDTO()
    {
        // Arrange
        var statisticsRepositoryMock = new Mock<IStatisticsRepository>();
        var expectedPaymentDistribution = new PaymentDistributionPerCurrency(
            new Dictionary<InvoicePaymentStatus, IEnumerable<Money>>
            {
                { InvoicePaymentStatus.Paid, new List<Money> { new(50, "USD"), new(75, "EUR") } },
                { InvoicePaymentStatus.Awaiting, new List<Money> { new(30, "USD"), new(45, "EUR") } }
            });

        statisticsRepositoryMock.Setup(repo => repo.GetPaymentStatusDistributionPerCurrencyAsync())
            .ReturnsAsync(expectedPaymentDistribution);

        var statisticsService = new StatisticsService(statisticsRepositoryMock.Object);

        // Act
        var result = await statisticsService.GetPaymentStatusDistributionPerCurrencyAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PaymentDistributionPerCurrencyDTO>(result);

        var expectedValues = new PaymentDistributionPerCurrencyDTO
        {
            Values = new Dictionary<ApiInvoicePaymentStatus, IEnumerable<MoneyDTO>>
            {
                {
                    ApiInvoicePaymentStatus.Paid, new List<MoneyDTO>
                        { new() { Amount = 50, CurrencyCode = "USD" }, new() { Amount = 75, CurrencyCode = "EUR" } }
                },
                {
                    ApiInvoicePaymentStatus.Awaiting, new List<MoneyDTO>
                        { new() { Amount = 30, CurrencyCode = "USD" }, new() { Amount = 45, CurrencyCode = "EUR" } }
                }
            }
        };


        Assert.Equal(expectedValues, result, new PaymentDistributionPerCurrencyDTOEqualityComparer());
    }

    private MoneyDTO MapToMoneyDTO(Money money)
    {
        return new MoneyDTO { CurrencyCode = money.CurrencyCode, Amount = money.Amount };
    }
}