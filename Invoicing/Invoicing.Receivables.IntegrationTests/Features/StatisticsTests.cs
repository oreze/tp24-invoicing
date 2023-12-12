using System.Net;
using System.Text;
using System.Text.Json;
using Identity.Receivables.ApplicationContracts.DTOs.Enums;
using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using Invoicing.Receivables.API;
using Invoicing.Receivables.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Invoicing.Receivables.IntegrationTests.Features;

public class StatisticsTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public StatisticsTests()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _httpClient = _webApplicationFactory.CreateDefaultClient();
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        _webApplicationFactory?.Dispose();
    }

    [Fact]
    public async Task Statistics_RetrieveValidTotalRevenuePerCurrencyValue()
    {
        var invoicesDTOs = new CreateInvoiceDTOFactory()
            .CreatePaidInvoice("USD", 1000)
            .CreatePaidInvoice("USD", 500)
            .CreatePaidInvoice("EUR", 1000)
            .CreatePaidInvoice("EUR", 600)
            .GetInvoiceDTOs();

        foreach (var dto in invoicesDTOs)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");
            var addInvoiceResponse = await _httpClient.PostAsync("/Receivables", jsonContent);
            Assert.Equal(HttpStatusCode.Created, addInvoiceResponse.StatusCode);
        }

        var getTotalRevenuePerCurrencyDto = await _httpClient.GetAsync("/Statistics/totalRevenuePerCurrency");

        Assert.Equal(HttpStatusCode.OK, getTotalRevenuePerCurrencyDto.StatusCode);
        var totalRevenuePerCurrencyDto =
            await HttpClientHelper.GetDeserializedResponseAsync<TotalRevenuePerCurrencyDTO>(getTotalRevenuePerCurrencyDto);

        Assert.Equal(1600, totalRevenuePerCurrencyDto.Values.First(m => m.CurrencyCode == "EUR").Amount);
        Assert.Equal(1500, totalRevenuePerCurrencyDto.Values.First(m => m.CurrencyCode == "USD").Amount);
    }

    [Fact]
    public async Task Statistics_RetrieveValidAverageTransactionValuePerCurrencyValue()
    {
        var invoicesDTOs = new CreateInvoiceDTOFactory()
            .CreatePaidInvoice("USD", 1000)
            .CreatePaidInvoice("USD", 500)
            .CreatePaidInvoice("EUR", 1000)
            .CreatePaidInvoice("EUR", 600)
            .GetInvoiceDTOs();

        foreach (var dto in invoicesDTOs)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");
            var addInvoiceResponse = await _httpClient.PostAsync("/Receivables", jsonContent);
            Assert.Equal(HttpStatusCode.Created, addInvoiceResponse.StatusCode);
        }

        var getAverageRevenuePerCurrencyDTO =
            await _httpClient.GetAsync("/Statistics/averageTransactionValuePerCurrency");

        Assert.Equal(HttpStatusCode.OK, getAverageRevenuePerCurrencyDTO.StatusCode);
        var averageRevenuePerCurrencyDTO =
            await HttpClientHelper.GetDeserializedResponseAsync<AverageTransactionValuePerCurrencyDTO>(getAverageRevenuePerCurrencyDTO);

        Assert.Equal(800, averageRevenuePerCurrencyDTO.Values.First(m => m.CurrencyCode == "EUR").Amount);
        Assert.Equal(750, averageRevenuePerCurrencyDTO.Values.First(m => m.CurrencyCode == "USD").Amount);
    }

    [Fact]
    public async Task Statistics_RetrieveValidPaymentDistributionPerCurrencyValue()
    {
        var invoicesDTOs = new CreateInvoiceDTOFactory()
            .CreatePaidInvoice("USD", 1000)
            .CreateClosedInvoice("USD", 1000)
            .CreateAwaitingInvoice("USD", 1000)
            .CreateCancelledInvoice("USD", 1000)
            .CreateOverdueInvoice("USD", 1000)
            .GetInvoiceDTOs();

        foreach (var dto in invoicesDTOs)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");
            var addInvoiceResponse = await _httpClient.PostAsync("/Receivables", jsonContent);
            Assert.Equal(HttpStatusCode.Created, addInvoiceResponse.StatusCode);
        }

        var getPaymentDistributionPerCurrencyDTO =
            await _httpClient.GetAsync("/Statistics/paymentDistributionPerCurrency");

        Assert.Equal(HttpStatusCode.OK, getPaymentDistributionPerCurrencyDTO.StatusCode);
        var paymentDistributionPerCurrencyDTO =
            await HttpClientHelper.GetDeserializedResponseAsync<PaymentDistributionPerCurrencyDTO>(getPaymentDistributionPerCurrencyDTO);

        AssertStatus(paymentDistributionPerCurrencyDTO, ApiInvoicePaymentStatus.Paid);
        AssertStatus(paymentDistributionPerCurrencyDTO, ApiInvoicePaymentStatus.Awaiting);
        AssertStatus(paymentDistributionPerCurrencyDTO, ApiInvoicePaymentStatus.Closed);
        AssertStatus(paymentDistributionPerCurrencyDTO, ApiInvoicePaymentStatus.Overdue);
        AssertStatus(paymentDistributionPerCurrencyDTO, ApiInvoicePaymentStatus.Canceled);
    }

    private void AssertStatus(PaymentDistributionPerCurrencyDTO paymentDistributionPerCurrencyDTO, ApiInvoicePaymentStatus status)
    {
        Assert.Collection(paymentDistributionPerCurrencyDTO.Values[status],
            item =>
            {
                Assert.Equal(1000, item.Amount, 2);
                Assert.Equal("USD", item.CurrencyCode);
            });
    }
}
