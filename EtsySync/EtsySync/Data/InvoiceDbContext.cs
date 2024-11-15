using Microsoft.EntityFrameworkCore;
using SharedProject.Models;

namespace EtsySync.Data
{
    public class InvoiceDbContext: DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options) { }

        public DbSet<SalesItem> SalesItems { get; set; }
        public DbSet<Client> Clients { get; set; } 
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}
