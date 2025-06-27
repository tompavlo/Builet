using Builet.BaseRepository;
using Builet.Database;
using Microsoft.EntityFrameworkCore;

namespace Builet.Stock;

public class StockRepository : Repository<Stock, long>, IStockRepository
{
    public StockRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Stock?> GetStockByCompanyNameAsync(string companyName)
    {
        return await _db.Stocks.FirstOrDefaultAsync(s => s.CompanyName == companyName);
    }

    public async Task<List<Stock>> GetAllStockBySectorAsync(string sector)
    {
        return await _db.Stocks.Where(s => s.Sector == sector).ToListAsync();
    }

    public async Task<Stock?> GetStockByTickerSymbol(string ticker)
    {
        return await _db.Stocks.FirstOrDefaultAsync(s => s.TickerSymbol == ticker);
    }
}