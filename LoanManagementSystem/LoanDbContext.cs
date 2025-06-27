using LoanManagementSystem.entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace LoanManagementSystem
{
    public class LoanDbContext : DbContext
    {
        public LoanDbContext() : base("name=LoanDbConnection") { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<HomeLoan> HomeLoans { get; set; }
        public DbSet<CarLoan> CarLoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HomeLoan>().Map(m => m.ToTable("HomeLoans"));
            modelBuilder.Entity<CarLoan>().Map(m => m.ToTable("CarLoans"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
