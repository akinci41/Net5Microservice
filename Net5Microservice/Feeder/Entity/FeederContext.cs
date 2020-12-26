using Microsoft.EntityFrameworkCore;

namespace Feeder.Entity
{
    public class FeederContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Startup.StaticConfig.GetSection("ConnectionStrings")["DefaultConnection"]);
        }
        public FeederContext() { }
        public FeederContext(DbContextOptions<FeederContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDetail> ReportDetails { get; set; }
    }
}
