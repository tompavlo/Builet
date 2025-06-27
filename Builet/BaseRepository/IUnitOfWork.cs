using Builet.Stock;
using Builet.Wallet;

namespace Builet.BaseRepository;

public interface IUnitOfWork : IDisposable
{
    IRepository<Inventory.Inventory, Guid> InventoryRepository { get; }
    IRepository<Transaction.Transaction, long> TransactionRepository { get; }
    IRepository<User.User, Guid> UserRepository { get; }
    
    IWalletRepository WalletRepository { get; }
    IStockRepository StockRepository { get; }

    Task<int> SaveAsync();
}