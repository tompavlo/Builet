using Builet.BaseRepository;

namespace Builet.Stock;

public interface IStockRepository : IRepository<Stock, long>
{
    Task<Stock?> GetStockByCompanyNameAsync(string companyName);
    Task<List<Stock>> GetAllStockBySectorAsync(string sector);
    Task<Stock?> GetStockByTickerSymbol(string ticker);
}