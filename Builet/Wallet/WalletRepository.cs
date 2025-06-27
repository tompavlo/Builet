using Builet.BaseRepository;
using Builet.Database;
using Microsoft.EntityFrameworkCore;

namespace Builet.Wallet;

public class WalletRepository : Repository<Wallet, Guid>, IWalletRepository
{
    public WalletRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<Wallet?> GetByUserIdAsync(Guid userId)
    {
        return await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
    }
}