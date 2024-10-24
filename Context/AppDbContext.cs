using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Addon> Addons { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<AccountProduct> AccountProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasKey(a => a.Id);

        modelBuilder.Entity<Addon>().HasKey(a => a.Id);
        modelBuilder.Entity<Addon>().HasOne(a => a.Product).WithMany(p => p.Addons).HasForeignKey(a => a.ProductId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>().HasKey(p => p.Id);

        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<AccountProduct>().HasKey(ap => new { ap.AccountId, ap.ProductId });
        modelBuilder.Entity<AccountProduct>().HasOne(ap => ap.Account).WithMany(a => a.Products).HasForeignKey(ap => ap.AccountId);
        modelBuilder.Entity<AccountProduct>().HasOne(ap => ap.Product).WithMany(p => p.Accounts).HasForeignKey(ap => ap.ProductId);
    }
}
