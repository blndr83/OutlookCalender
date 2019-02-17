using Microsoft.EntityFrameworkCore;
using Models;

namespace StorageProvider
{
    public class CalendarDbContext : DbContext
    {
        public static string DbPath { get; set; }
        public const string DB = "Calendar.db";
        public CalendarDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var datasource = string.IsNullOrEmpty(DbPath) ? DB : DbPath;
            optionsBuilder.UseSqlite($"Data Source={datasource}");
        }

        public DbSet<EventModel> Events { get; set; }
    }
}
