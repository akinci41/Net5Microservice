using Microsoft.EntityFrameworkCore;

namespace Directory.Entity
{
    public class DirectoryContext : DbContext
    {
        public DirectoryContext(DbContextOptions<DirectoryContext> options) : base(options) { }
        public DbSet<Entity.Contact> Contacts { get; set; }
        public DbSet<Entity.Communication> Communications { get; set; }
    }
}
