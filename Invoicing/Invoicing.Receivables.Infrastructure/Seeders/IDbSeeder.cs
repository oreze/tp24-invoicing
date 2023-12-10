namespace Invoicing.Receivables.Infrastructure.Seeders;

public interface IDbSeeder
{
    public Task EnsureSeedDatabase(WebApplication app);
}