using Identity.Receivables.ApplicationContracts.DTOs;
using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Domain.Exceptions;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Currency;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Debtor;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Invoice;

namespace Invoicing.Receivables.Infrastructure.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IDebtorRepository _debtorRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository, IDebtorRepository debtorRepository,
        ICurrencyRepository currencyRepository)
    {
        _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
        _debtorRepository = debtorRepository ?? throw new ArgumentNullException(nameof(debtorRepository));
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
    }

    public async Task<GetInvoiceDTO> GetInvoiceByIdAsync(int invoiceId)
    {
        var invoiceEntity = await _invoiceRepository.GetByIdAsync(invoiceId);

        if (invoiceEntity == null) throw new NotFoundException($"Invoice with ID {invoiceId} not found.");

        return CreateGetInvoiceDTO(invoiceEntity);
    }

    public async Task<int> CreateInvoiceAsync(CreateInvoiceDTO createInvoiceDto)
    {
        var currency = await _currencyRepository.GetByCodeAsync(createInvoiceDto.CurrencyCode)
                       ?? throw new NotFoundException($"Invoice currency code {createInvoiceDto.CurrencyCode} was not found.");

        var debtor = await _debtorRepository.GetByReferenceAsync(createInvoiceDto.DebtorReference)
                     ?? CreateDebtor(createInvoiceDto);

        var newInvoiceEntity = CreateInvoice(createInvoiceDto, debtor, currency);

        await _invoiceRepository.AddAsync(newInvoiceEntity);
        await _invoiceRepository.SaveChangesAsync();

        return newInvoiceEntity.ID;
    }

    private GetInvoiceDTO CreateGetInvoiceDTO(Invoice invoice)
    {
        return new GetInvoiceDTO
        {
            InvoiceID = invoice.ID,
            Reference = invoice.Reference,
            CurrencyCode = invoice.Currency?.Code,
            IssueDate = invoice.IssueDate.ToString("yyyy-MM-dd"),
            OpeningValue = invoice.OpeningValue,
            PaidValue = invoice.PaidValue,
            DueDate = invoice.DueDate.ToString("yyyy-MM-dd"),
            ClosedDate = invoice.ClosedDate,
            Cancelled = invoice.Cancelled,
            Debtor = MapToDTO(invoice.Debtor),
            Currency = MapToDTO(invoice.Currency)
        };
    }

    private DebtorDTO MapToDTO(Debtor debtor)
    {
        return new DebtorDTO
        {
            Reference = debtor.Reference,
            ID = debtor.ID,
            Address1 = debtor.Address1,
            Address2 = debtor.Address2,
            Name = debtor.Name,
            State = debtor.State,
            Town = debtor.State,
            Zip = debtor.Zip,
            CountryCode = debtor.CountryCode,
            RegistrationNumber = debtor.RegistrationNumber
        };
    }

    private CurrencyDTO MapToDTO(Currency currency)
    {
        return new CurrencyDTO
        {
            Code = currency.Code,
            Name = currency.Name
        };
    }

    private Invoice CreateInvoice(CreateInvoiceDTO createInvoiceDto, Debtor debtor, Currency currency)
    {
        return Invoice.Create(createInvoiceDto.Reference, createInvoiceDto.IssueDate, createInvoiceDto.OpeningValue,
            createInvoiceDto.PaidValue, createInvoiceDto.DueDate, createInvoiceDto.ClosedDate, createInvoiceDto.Cancelled,
            debtor,
            currency);
    }

    private Debtor CreateDebtor(CreateInvoiceDTO createInvoiceDto)
    {
        return Debtor.Create(createInvoiceDto.DebtorReference, createInvoiceDto.DebtorName, createInvoiceDto.DebtorAddress1,
            createInvoiceDto.DebtorAddress2, createInvoiceDto.DebtorTown, createInvoiceDto.DebtorState,
            createInvoiceDto.DebtorZip, createInvoiceDto.DebtorCountryCode, createInvoiceDto.DebtorRegistrationNumber);
    }
}