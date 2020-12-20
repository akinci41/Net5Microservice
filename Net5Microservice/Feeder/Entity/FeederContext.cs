using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Feeder.Entity
{
    public class FeederContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"];
            optionsBuilder.UseNpgsql(cs);
        }
        public FeederContext() { }
        public FeederContext(DbContextOptions<FeederContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDetail> ReportDetails { get; set; }
    }
}
