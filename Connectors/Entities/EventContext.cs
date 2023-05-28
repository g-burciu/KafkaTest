using Microsoft.EntityFrameworkCore;

namespace Connectors.Entities
{
    public class EventContext : DbContext
    {
        public EventContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlServer($"HIDDEN");

        public DbSet<Event> Events { get; set; }

    }
}
