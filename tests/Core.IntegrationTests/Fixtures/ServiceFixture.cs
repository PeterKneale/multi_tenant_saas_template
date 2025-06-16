using Core.Infrastructure;
using Core.Infrastructure.Database;
using Core.Infrastructure.Database.Migrations;
using MartinCostello.Logging.XUnit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.IntegrationTests.Fixtures;

public class ServiceFixture : IDisposable, ITestOutputHelperAccessor
{
    private static readonly SemaphoreSlim MigrationLock = new(1, 1);
    private static bool _migrated;
    private readonly ServiceProvider _provider;

    public ServiceFixture()
    {
        var configuration = new ConfigurationBuilder()
//AddJsonFile("appsettings.json", false)
            .AddJsonFile("testsettings.json", false)
            .AddEnvironmentVariables()
            .Build();

        _provider = new ServiceCollection()
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddLogging(builder => builder.AddXUnit(this, c =>
            {
                c.Filter = (category, level) =>
                {
                    if (category.Contains("Microsoft.EntityFrameworkCore")) return level >= LogLevel.Warning;

                    return level >= LogLevel.Information;
                };
            }))
            .AddScoped<ICurrentContext, FakeCurrentContext>()
            .BuildServiceProvider();

        EnsureDatabaseMigrated();
    }

    public IServiceProvider ServiceProvider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }

    public ITestOutputHelper? OutputHelper { get; set; }

    private void EnsureDatabaseMigrated()
    {
        MigrationLock.Wait();
        try
        {
            if (!_migrated)
            {
                _provider.ApplyDatabaseMigrations(true);
                _migrated = true;
            }
        }
        finally
        {
            MigrationLock.Release();
        }
    }

    public async Task ExecuteSql(string sql)
    {
        var database = ServiceProvider.GetRequiredService<DatabaseContext>();
        await database.Database.ExecuteSqlRawAsync(sql);
    }
}