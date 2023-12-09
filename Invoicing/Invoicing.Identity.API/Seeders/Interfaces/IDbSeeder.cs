namespace Invoicing.Identity.API.Seeders;

public interface IDbSeeder
{
    public Task EnsureSeedData(WebApplication app);
}