using Builet.User;
using Microsoft.EntityFrameworkCore;

namespace Builet.Database;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction.Transaction>()
            .HasOne(t => t.Seller)
            .WithMany(u => u.TransactionsAsSeller)
            .HasForeignKey(t => t.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction.Transaction>()
            .HasOne(t => t.Buyer)
            .WithMany(u => u.TransactionsAsBuyer)
            .HasForeignKey(t => t.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User.User>().HasData(
            new User.User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Username = "admin",
                Email = "admin@admin.com",
                PasswordHash = "$2a$12$EUxb19mxNgqWshd58CLGne.5cmV7Jkad/lEiJ9tu7.hm9FxGd8.x6",
                Role = Role.Admin
            });

    }

    public DbSet<Stock.Stock> Stocks { get; set; } = null!;
    public DbSet<Inventory.Inventory> Inventories { get; set; } = null!;
    public DbSet<Transaction.Transaction> Transactions { get; set; } = null!;
    public DbSet<User.User> Users { get; set; } = null!;
    public DbSet<Wallet.Wallet> Wallets { get; set; } = null!;
}