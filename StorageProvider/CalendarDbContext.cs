using Microsoft.EntityFrameworkCore;
using Models;

namespace StorageProvider
{
    public class CalendarDbContext : DbContext
    {
        public CalendarDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<EventModel> Events { get; set; }
        public DbSet<SyncLog> SyncLogs { get; set; }
    }
}
