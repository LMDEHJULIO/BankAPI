using bankapi.models;
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

        //public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Deposit> Deposits { get; set; }

        public DbSet<Withdrawal> Withdrawals { get; set; }
        public DbSet<Bill> Bills { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Address);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(a => a.AccountId);


            modelBuilder.Entity<Transaction>()
                .HasDiscriminator<string>("TransactionType")
                .HasValue<Withdrawal>(nameof(TransactionType.Withdrawal))
                .HasValue<Deposit>(nameof(TransactionType.Deposit))
                .HasValue<P2P>(nameof(TransactionType.P2P));

            base.OnModelCreating(modelBuilder); 
        }

    }
}
