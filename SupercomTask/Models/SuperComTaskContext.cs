using Microsoft.EntityFrameworkCore;

namespace SupercomTask.Models
{
    public class SuperComTaskContext : DbContext
    {
        public SuperComTaskContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ExpirationPublished> ExpirationPublished { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().ToTable("Cards", c => c.IsTemporal());
            modelBuilder.Entity<Status>().ToTable("Status", c => c.IsTemporal());
            modelBuilder.Entity<ExpirationPublished>().ToTable("ExpirationPublished", c => c.IsTemporal());
        }
    }
}
