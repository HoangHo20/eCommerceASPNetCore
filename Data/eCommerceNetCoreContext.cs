using eCommerceASPNetCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceASPNetCore.Data
{
    public class eCommerceNetCoreContext : DbContext
    {
        public eCommerceNetCoreContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        private string _connectionString;
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public string V { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer(_connectionString)
                .UseLazyLoadingProxies()
                .LogTo(Console.WriteLine, new[] {
                    DbLoggerCategory.Model.Name,
                    DbLoggerCategory.Database.Command.Name,
                    DbLoggerCategory.Database.Transaction.Name,
                    DbLoggerCategory.Query.Name,
                    DbLoggerCategory.ChangeTracking.Name,
                },
                       LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
}
