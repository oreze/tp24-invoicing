using Microsoft.EntityFrameworkCore;

namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Debtor;

public class DebtorRepository : IDebtorRepository
{
    private readonly AppDbContext _dbContext;

    public DebtorRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Domain.Entities.Debtor?> GetByReferenceAsync(string reference)
    {
        return await _dbContext.Debtors.FirstOrDefaultAsync(d => d.Reference == reference);
    }

    public async Task AddAsync(Domain.Entities.Debtor debtor)
    {
        var doesDebtorExists = await _dbContext.Debtors.AnyAsync(d => d.ID == debtor.ID);

        if (!doesDebtorExists) await _dbContext.Debtors.AddAsync(debtor);
    }
    
    public async Task AddRangeAsync(IEnumerable<Domain.Entities.Debtor> debtors)
    {
        IEnumerable<Domain.Entities.Debtor> missingRecords = 
            debtors.Where(x => !_dbContext.Debtors.Any(z => z.Reference == x.Reference));

        await _dbContext.Debtors.AddRangeAsync(missingRecords);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
