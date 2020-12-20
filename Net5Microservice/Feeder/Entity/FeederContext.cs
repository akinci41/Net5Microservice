using Microsoft.EntityFrameworkCore;

namespace Feeder.Entity
{
    public class FeederContext : DbContext
    {
        public FeederContext(DbContextOptions<FeederContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Communication> Communications { get; set; }
    }
}
