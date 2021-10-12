/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.Interfaces;
using Headtrip.Models;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace Headtrip.Data
{
    public class SqliteContext : DbContext
    {
        public SqliteContext() { }

        public SqliteContext(DbContextOptions options) : base(options) {  }

        public virtual DbSet<LogEntry> LogEntrys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string sqliteFilePath = DependencyService.Get<ISqliteService>().GetDbPath();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Filename={sqliteFilePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().HasIndex(x => x.ID).IsUnique();
        }
    }
}