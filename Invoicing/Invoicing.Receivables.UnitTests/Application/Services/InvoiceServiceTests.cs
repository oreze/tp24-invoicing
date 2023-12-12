using Identity.Receivables.ApplicationContracts.DTOs.Invoices;
using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Domain.Exceptions;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Currency;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Debtor;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Invoice;
using Invoicing.Receivables.Infrastructure.Services;
using Invoicing.Receivables.UnitTests.Common.Helpers;
using Moq;

namespace Invoicing.Receivables.UnitTests.Application.Services;

public class InvoiceServiceTests
{
    [Fact]
    public async Task GetInvoiceByIdAsync_ValidId_ReturnsGetInvoiceDTO()
    {
        // Arrange
        var invoiceId = 1;
        var expectedInvoice = DefaultEntities.GetDefaultInvoice();

        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        invoiceRepositoryMock.Setup(repo => repo.GetByIdAsync(invoiceId)).ReturnsAsync(expectedInvoice);

        var debtorRepositoryMock = new Mock<IDebtorRepository>();
        var currencyRepositoryMock = new Mock<ICurrencyRepository>();

        var invoiceService = new InvoiceService(invoiceRepositoryMock.Object, debtorRepositoryMock.Object, currencyRepositoryMock.Object);

        // Act
        var result = await invoiceService.GetInvoiceByIdAsync(invoiceId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedInvoice.ID, result.InvoiceID);
        Assert.Equal(expectedInvoice.Reference, result.Reference);
        // Assert other properties

        // Verify that the GetByIdAsync method was called with the correct ID
        invoiceRepositoryMock.Verify(repo => repo.GetByIdAsync(invoiceId), Times.Once);
    }

    [Fact]
    public async Task GetInvoiceByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var invalidInvoiceId = int.MaxValue;

        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        invoiceRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidInvoiceId)).ReturnsAsync((Invoice)null);

        var debtorRepositoryMock = new Mock<IDebtorRepository>();
        var currencyRepositoryMock = new Mock<ICurrencyRepository>();

        var invoiceService = new InvoiceService(invoiceRepositoryMock.Object, debtorRepositoryMock.Object, currencyRepositoryMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => invoiceService.GetInvoiceByIdAsync(invalidInvoiceId));

        invoiceRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidInvoiceId), Times.Once);
    }

    [Fact]
    public async Task CreateInvoiceAsync_ValidInput_ReturnsNewInvoiceId()
    {
        // Arrange
        var createInvoiceDto = GetDefaultCreateInvoiceDTO();

        var currency = DefaultEntities.GetDefaultCurrency();
        var debtor = DefaultEntities.GetDefaultDebtor();
        var newInvoiceId = 123;

        var currencyRepositoryMock = new Mock<ICurrencyRepository>();
        currencyRepositoryMock.Setup(repo => repo.GetByCodeAsync(createInvoiceDto.CurrencyCode)).ReturnsAsync(currency);

        var debtorRepositoryMock = new Mock<IDebtorRepository>();
        debtorRepositoryMock.Setup(repo => repo.GetByReferenceAsync(createInvoiceDto.DebtorReference)).ReturnsAsync(debtor);

        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        invoiceRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Invoice>())).Callback<Invoice>(invoice =>
        {
            // Simulate the behavior of EF Core saving the entity and updating the ID
            invoice.GetType().GetProperty("ID")?.SetValue(invoice, newInvoiceId, null);
        });

        var invoiceService = new InvoiceService(invoiceRepositoryMock.Object, debtorRepositoryMock.Object, currencyRepositoryMock.Object);

        // Act
        var result = await invoiceService.CreateInvoiceAsync(createInvoiceDto);

        // Assert
        Assert.Equal(newInvoiceId, result);

        currencyRepositoryMock.Verify(repo => repo.GetByCodeAsync(createInvoiceDto.CurrencyCode), Times.Once);
        debtorRepositoryMock.Verify(repo => repo.GetByReferenceAsync(createInvoiceDto.DebtorReference), Times.Once);
        invoiceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Invoice>()), Times.Once);
        invoiceRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateInvoiceAsync_InvalidCurrency_ThrowsNotFoundException()
    {
        // Arrange
        var createInvoiceDto = GetDefaultCreateInvoiceDTO();

        var currencyRepositoryMock = new Mock<ICurrencyRepository>();
        currencyRepositoryMock.Setup(repo => repo.GetByCodeAsync(createInvoiceDto.CurrencyCode)).ReturnsAsync((Currency)null);

        var debtorRepositoryMock = new Mock<IDebtorRepository>();
        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();

        var invoiceService = new InvoiceService(invoiceRepositoryMock.Object, debtorRepositoryMock.Object, currencyRepositoryMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<InputException>(() => invoiceService.CreateInvoiceAsync(createInvoiceDto));

        // Verify that the repositories' methods were called with the correct parameters
        currencyRepositoryMock.Verify(repo => repo.GetByCodeAsync(createInvoiceDto.CurrencyCode), Times.Once);
        debtorRepositoryMock.Verify(repo => repo.GetByReferenceAsync(It.IsAny<string>()), Times.Never);
        invoiceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Invoice>()), Times.Never);
        invoiceRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateInvoiceAsync_DebtorNotFound_CreatesNewDebtor()
    {
        // Arrange
        var createInvoiceDto = GetDefaultCreateInvoiceDTO();

        var currency = DefaultEntities.GetDefaultCurrency();
        var newInvoiceId = 123;

        var currencyRepositoryMock = new Mock<ICurrencyRepository>();
        currencyRepositoryMock.Setup(repo => repo.GetByCodeAsync(createInvoiceDto.CurrencyCode)).ReturnsAsync(currency);

        var debtorRepositoryMock = new Mock<IDebtorRepository>();
        debtorRepositoryMock.Setup(repo => repo.GetByReferenceAsync(createInvoiceDto.DebtorReference)).ReturnsAsync((Debtor)null);

        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        invoiceRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Invoice>())).Callback<Invoice>(invoice =>
        {
            // Simulate the behavior of EF Core saving the entity and updating the ID
            invoice.GetType().GetProperty("ID")?.SetValue(invoice, newInvoiceId, null);
        });

        var invoiceService = new InvoiceService(invoiceRepositoryMock.Object, debtorRepositoryMock.Object, currencyRepositoryMock.Object);

        // Act
        var result = await invoiceService.CreateInvoiceAsync(createInvoiceDto);

        // Assert
        Assert.Equal(newInvoiceId, result);

        // Verify that the repositories' methods were called with the correct parameters
        currencyRepositoryMock.Verify(repo => repo.GetByCodeAsync(createInvoiceDto.CurrencyCode), Times.Once);
        debtorRepositoryMock.Verify(repo => repo.GetByReferenceAsync(createInvoiceDto.DebtorReference), Times.Once);
        invoiceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Invoice>()), Times.Once);
        invoiceRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    private CreateInvoiceDTO GetDefaultCreateInvoiceDTO()
    {
        return new CreateInvoiceDTO
        {
            Reference = "INV123",
            CurrencyCode = "USD",
            IssueDate = DateTime.Now,
            OpeningValue = 1000.00m,
            PaidValue = 500.00m,
            DueDate = DateTime.Now.AddDays(30),
            ClosedDate = null,
            Cancelled = null,
            DebtorName = "John Doe",
            DebtorReference = "DEB001",
            DebtorAddress1 = "123 Main St",
            DebtorAddress2 = "Apt 456",
            DebtorTown = "Cityville",
            DebtorState = "State",
            DebtorZip = "12345",
            DebtorCountryCode = "US",
            DebtorRegistrationNumber = "ABC123"
        };
    }
}