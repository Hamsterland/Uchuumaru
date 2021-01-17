using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Data
{
    /// <summary>
    /// The application database context.
    /// </summary>
    public class UchuumaruContext : DbContext
    {
        /// <summary>
        /// The Guilds the bot is in.
        /// </summary>
        public DbSet<Guild> Guilds { get; set; }
        
        /// <summary>
        /// The channels the bot has access to.
        /// </summary>
        public DbSet<Channel> Channels { get; set; }
        
        /// <summary>
        /// The active users who share guilds with the bot.
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// Constructs a new <see cref="UchuumaruContext"/>
        /// </summary>
        /// <param name="options">The context options.</param>
        public UchuumaruContext(DbContextOptions options) : base(options)
        { }
        
        /// <summary>
        /// Applies model configurations from configuration classes that
        /// implement <see cref="IEntityTypeConfiguration{TEntity}"/>.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        }
    }
}