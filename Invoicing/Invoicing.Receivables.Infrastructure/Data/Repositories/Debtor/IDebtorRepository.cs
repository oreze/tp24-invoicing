namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Debtor;

public interface IDebtorRepository
{
    Task<Domain.Entities.Debtor?> GetByReferenceAsync(string reference);
    Task AddAsync(Domain.Entities.Debtor debtor);
    Task AddRangeAsync(IEnumerable<Domain.Entities.Debtor> debtor);
    Task<int> SaveChangesAsync();
}
