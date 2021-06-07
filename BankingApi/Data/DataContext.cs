using BankingApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        public DbSet<BankAccount> BankAccounts { get; set; } 
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasMany(a => a.BankAccounts)
                .WithOne(c => c.Customer)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BankAccount>()
                .HasMany(i => i.Income)
                .WithOne(s => s.SenderAccount)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<BankAccount>()
                .HasMany(o => o.Outcome)
                .WithOne(r => r.RecipientAccount)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}