using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Queries;

public record GetPaymentDistributionPerCurrencyQuery : IRequest<PaymentDistributionPerCurrencyDTO>;