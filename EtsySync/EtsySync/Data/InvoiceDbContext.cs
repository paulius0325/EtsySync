﻿
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedProject.Models;

namespace EtsySync.Data
{
    public class InvoiceDbContext : IdentityDbContext<ApplicationUser>
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options) { }

        public DbSet<SalesItem> SalesItems { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<EncryptionKey> EncryptionKeys { get; set; }
        public DbSet<CompressedFile> ZipFiles { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompressedFile>()
                .HasKey(z => z.ZipFileId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
