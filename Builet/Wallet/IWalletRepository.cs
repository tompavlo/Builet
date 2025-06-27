using Builet.BaseRepository;

namespace Builet.Wallet;

public interface IWalletRepository : IRepository<Wallet, Guid>
{
    Task<Wallet?> GetByUserIdAsync(Guid userId);
}