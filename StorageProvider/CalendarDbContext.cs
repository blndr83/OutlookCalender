using Microsoft.EntityFrameworkCore;
using Models;

namespace StorageProvider
{
    public class CalendarDbContext : DbContext
    {
        public CalendarDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = "Calendar.db";
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        public DbSet<EventModel> Events { get; set; }
    }
}
