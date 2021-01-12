using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Uchuumaru.Data
{
    public class UchuumaruContextFactory : IDesignTimeDbContextFactory<UchuumaruContext>
    {
        public UchuumaruContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<UchuumaruContext>()
                .Build();
            
            var optionsBuilder = new DbContextOptionsBuilder()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseNpgsql(configuration["Postgres:Connection"]);
            
            return new UchuumaruContext(optionsBuilder.Options);
        }
    }
}