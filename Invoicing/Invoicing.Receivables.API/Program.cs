using Invoicing.Receivables.API.Middlewares;
using Invoicing.Receivables.Infrastructure.Data;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Currency;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Debtor;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Invoice;
using Invoicing.Receivables.Infrastructure.Extensions;
using Invoicing.Receivables.Infrastructure.Seeders;
using Invoicing.Receivables.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, lc) => lc
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(context.Configuration));

var connectionString = builder.Configuration.GetConnectionString("ReceivablesDB")
                       ?? throw new ArgumentNullException("ReceivablesDB connection string is null.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IDebtorRepository, DebtorRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IDbSeeder, DbSeeder>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<CustomExceptionHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsSystemTests())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbSeeder = scope.ServiceProvider.GetService<IDbSeeder>() ?? throw new ArgumentNullException(nameof(IDbSeeder));
    await dbSeeder.EnsureSeedDatabase(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
