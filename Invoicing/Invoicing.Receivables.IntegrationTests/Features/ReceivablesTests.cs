using System.Net;
using System.Text;
using System.Text.Json;
using Identity.Receivables.ApplicationContracts.DTOs.Invoices;
using Invoicing.Receivables.API;
using Invoicing.Receivables.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Invoicing.Receivables.IntegrationTests.Features;

public class ReceivablesTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public ReceivablesTests()
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
    public async Task Invoice_CanRetrieveInvoiceInvoiceByID()
    {
        var invoiceToBeAdded = DefaultEntities.GetDefaultCreateInvoiceDTO();

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(invoiceToBeAdded),
            Encoding.UTF8,
            "application/json");
        var addInvoiceResponse = await _httpClient.PostAsync("/Receivables", jsonContent);

        Assert.Equal(HttpStatusCode.Created, addInvoiceResponse.StatusCode);

        var newInvoiceID = await addInvoiceResponse.Content.ReadAsStringAsync();

        var getInvoiceResponse = await _httpClient.GetAsync($"/Receivables/{newInvoiceID}");

        Assert.Equal(HttpStatusCode.OK, getInvoiceResponse.StatusCode);
        var retrievedInvoiceDTO = await HttpClientHelper.GetDeserializedResponseAsync<GetInvoiceDTO>(getInvoiceResponse);

        Assert.Equal(invoiceToBeAdded.Reference, retrievedInvoiceDTO.Reference);
        Assert.Equal(invoiceToBeAdded.CurrencyCode, retrievedInvoiceDTO.CurrencyCode);
        Assert.Equal(invoiceToBeAdded.IssueDate.ToString("yyyy-MM-dd"), retrievedInvoiceDTO.IssueDate);
        Assert.Equal(invoiceToBeAdded.OpeningValue, retrievedInvoiceDTO.OpeningValue);
        Assert.Equal(invoiceToBeAdded.PaidValue, retrievedInvoiceDTO.PaidValue);
        Assert.Equal(invoiceToBeAdded.DueDate.ToString("yyyy-MM-dd"), retrievedInvoiceDTO.DueDate);
        Assert.Equal(invoiceToBeAdded.ClosedDate, retrievedInvoiceDTO.ClosedDate);
        Assert.Equal(invoiceToBeAdded.Cancelled, retrievedInvoiceDTO.Cancelled);
        Assert.Equal(invoiceToBeAdded.DebtorName, retrievedInvoiceDTO.Debtor.Name);
        Assert.Equal(invoiceToBeAdded.DebtorReference, retrievedInvoiceDTO.Debtor.Reference);
        Assert.Equal(invoiceToBeAdded.DebtorAddress1, retrievedInvoiceDTO.Debtor.Address1);
        Assert.Equal(invoiceToBeAdded.DebtorAddress2, retrievedInvoiceDTO.Debtor.Address2);
        Assert.Equal(invoiceToBeAdded.DebtorTown, retrievedInvoiceDTO.Debtor.Town);
        Assert.Equal(invoiceToBeAdded.DebtorState, retrievedInvoiceDTO.Debtor.State);
        Assert.Equal(invoiceToBeAdded.DebtorZip, retrievedInvoiceDTO.Debtor.Zip);
        Assert.Equal(invoiceToBeAdded.DebtorCountryCode, retrievedInvoiceDTO.Debtor.CountryCode);
        Assert.Equal(invoiceToBeAdded.DebtorRegistrationNumber, retrievedInvoiceDTO.Debtor.RegistrationNumber);
    }
}
