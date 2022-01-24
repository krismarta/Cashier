using Cashier.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RequestGoods> RequestGoods { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DetailRequest> DetailRequests { get; set; }
        public DbSet<DetailTransaction> DetailTransactions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Account)
                .WithOne(u => u.User)
                .HasForeignKey<Account>(b => b.id);

            modelBuilder.Entity<Account>()
                .HasOne(r => r.Role)
                .WithMany(a => a.Accounts);

            modelBuilder.Entity<Transaction>()
                .HasOne(u => u.User)
                .WithMany(t => t.Transactions);

            modelBuilder.Entity<DetailTransaction>()
                .HasOne(t => t.Transaction)
                .WithMany(dt => dt.DetailTransactions);

            modelBuilder.Entity<DetailTransaction>()
                .HasOne(b => b.Goods)
                .WithMany(dt => dt.DetailTransactions);

            modelBuilder.Entity<Goods>()
                .HasOne(s => s.Supplier)
                .WithMany(g => g.Goods);

            modelBuilder.Entity<Goods>()
                .HasOne(c => c.Category)
                .WithMany(g => g.Goods);

            modelBuilder.Entity<RequestGoods>()
                .HasOne(u => u.User)
                .WithMany(rg => rg.RequestGoods);

            modelBuilder.Entity<RequestGoods>()
                .HasOne(s => s.Supplier)
                .WithMany(rg => rg.RequestGoods);

            modelBuilder.Entity<DetailRequest>()
                .HasOne(rg => rg.RequestGoods)
                .WithMany(dr => dr.DetailRequests);

            modelBuilder.Entity<DetailRequest>()
                .HasOne(g => g.Goods)
                .WithMany(dr => dr.DetailRequests);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

}

}