using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Queries;

public record GetTotalRevenuePerCurrencyQuery : IRequest<TotalRevenuePerCurrencyDTO>;