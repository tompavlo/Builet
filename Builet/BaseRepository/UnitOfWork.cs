using Builet.Database;
using Builet.Stock;
using Builet.Wallet;

namespace Builet.BaseRepository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public IRepository<Inventory.Inventory, Guid> InventoryRepository { get; }
    public IRepository<Transaction.Transaction, long> TransactionRepository { get; }
    public IRepository<User.User, Guid> UserRepository { get; }

    public IWalletRepository WalletRepository { get; }
    public IStockRepository StockRepository { get; }

    public UnitOfWork(AppDbContext db)
    {
        _db = db;

        InventoryRepository = new Repository<Inventory.Inventory, Guid>(_db);
        TransactionRepository = new Repository<Transaction.Transaction, long>(_db);
        UserRepository = new Repository<User.User, Guid>(_db);
        WalletRepository = new WalletRepository(_db);
        StockRepository = new StockRepository(_db);
    }

    public async Task<int> SaveAsync()
    {
       return await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}