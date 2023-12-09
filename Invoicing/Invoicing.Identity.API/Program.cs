using System.Reflection;
using Invoicing.Identity.API.Configuration;
using Invoicing.Identity.API.Configuration.Interfaces.Interfaces;
using Invoicing.Identity.API.Extensions;
using Invoicing.Identity.API.Seeders;
using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Infrastructure.Configuration.Identity;
using Invoicing.Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, lc) => lc
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

var connectionString = builder.Configuration.GetConnectionString("IdentityDB")
                       ?? throw new ArgumentNullException("IdentityDB connection string is null.");
var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name
                         ?? throw new ArgumentNullException("migrationsAssembly");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;

        options.EmitStaticAudienceClaim = true;
    })
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
    })
    .AddProfileService<CustomProfileService>()
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbSeeder, DbSeeder>();
builder.Services.AddScoped<IDefaultEntitiesProvider, DefaultEntitiesProvider>();
builder.Services.AddScoped<IDefaultIdentityConfigurationProvider, DefaultIdentityConfigurationProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment() || app.Environment.IsSystemTests())
{
    using var scope = app.Services.CreateScope();
    var dbSeeder = scope.ServiceProvider.GetService<IDbSeeder>() ?? throw new ArgumentNullException(nameof(IDbSeeder));
    await dbSeeder.EnsureSeedData(app);
}

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();

public partial class Program
{
}