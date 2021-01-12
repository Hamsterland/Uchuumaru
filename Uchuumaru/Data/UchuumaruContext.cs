using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Data
{
    public class UchuumaruContext : DbContext
    {
        public List<Guild> Guilds { get; set; }
        public List<Channel> Channels { get; set; }
        public List<User> Users { get; set; }
        
        public UchuumaruContext(DbContextOptions options) : base(options)
        { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        }
    }
}