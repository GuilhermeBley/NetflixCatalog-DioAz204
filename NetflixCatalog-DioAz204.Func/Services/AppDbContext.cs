using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetflixCatalog_DioAz204.Func.Model;

namespace NetflixCatalog_DioAz204.Func.Services;

public class AppDbContext : DbContext
{
    private readonly CosmosDbOption _option;

    public DbSet<MovieModel> Customers { get; set; }

    public AppDbContext(IOptions<CosmosDbOption> options)
    {
        _option = options.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            accountEndpoint: _option.Endpoint,
            accountKey: _option.AccountKey,
            databaseName: _option.DataBaseName
        ).UseAsyncSeeding((context, _, cancellationToken) =>
        {
            return context.Database.EnsureCreatedAsync(cancellationToken);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieModel>().ToContainer("Movies");
        modelBuilder.Entity<MovieModel>().HasPartitionKey(c => c.Id);
    }
}
