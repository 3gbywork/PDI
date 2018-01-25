using Microsoft.EntityFrameworkCore;
using PDI.NetCoreDemos.Models;

namespace PDI.NetCoreDemos.Data
{
    public class VCardContext : DbContext
    {
        public VCardContext(DbContextOptions<VCardContext> options) : base(options)
        {
        }

        public DbSet<VCardDto> VCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VCardDto>().ToTable("VCardDTO");
        }
    }
}
