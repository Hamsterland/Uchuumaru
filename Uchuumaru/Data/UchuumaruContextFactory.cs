using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Uchuumaru.Data
{
    /// <summary>
    /// The design-time context factory for the application.
    /// </summary>
    public class UchuumaruContextFactory : IDesignTimeDbContextFactory<UchuumaruContext>
    {
        /// <summary>
        /// Creates a new <see cref="UchuumaruContext"/> at design time.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// A newly created <see cref="UchuumaruContext"/> at design time.
        /// </returns>
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