using eCommerceASPNetCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace eCommerceASPNetCore.Data
{
    public class eCommerceNetCoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=eCommerceASPNetCore;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer(@"Server=DESKTOP-92B5VRS\SQLEXPRESS;Database=eCommerceNetCore;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=true");
        }
    }
}
