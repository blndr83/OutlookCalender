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
            optionsBuilder.UseSqlite("Data Source=Calendar.db");
        }

        public DbSet<EventModel> Events { get; set; }
    }
}
