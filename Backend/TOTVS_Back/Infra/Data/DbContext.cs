using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price);
                entity.Property(e => e.Description);
                entity.Property(e => e.SKU);
                entity.Property(e => e.Image);
                entity.Property(e => e.Excluido);
                entity.Property(e => e.CreateTs);
                entity.Property(e => e.ModTs);
            });

            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.Excluido);
        }
    }

}
