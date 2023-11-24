using BankAPI.Models;
using IBM.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Address);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId);
          

            base.OnModelCreating(modelBuilder);
        }

    }
}
