using System.Xml;
using AutoMapper;
using Builet.BaseRepository;

namespace Builet.Stock;

public class StockService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StockService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Stock> CreateStockAsync(CreateStockDto dto)
    {
        var stockExist = await _unitOfWork.StockRepository.FindAsync(stck => stck.TickerSymbol == dto.TickerSymbol
                                                                             || stck.CompanyName == dto.CompanyName);
        if (stockExist.Any()) throw new Exception("There is already such stock");

        var stock = new Stock
        {
            TickerSymbol = dto.TickerSymbol,
            CompanyName = dto.CompanyName,
            Sector = dto.Field
        };

        await _unitOfWork.StockRepository.AddAsync(stock);
        await _unitOfWork.SaveAsync();

        return stock;
    }

    public async Task DeleteStockAsync(long stockId)
    {
        var stock = await _unitOfWork.StockRepository.GetAsync(stockId);
        if (stock == null)
            throw new Exception("Stock not found");

        _unitOfWork.StockRepository.Remove(stock);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Stock> FindById(long id)
    {
        var stock = await _unitOfWork.StockRepository.GetAsync(id);
        if (stock == null)
            throw new Exception("Stock not found");
        return stock;
    }

    public async Task<List<Stock>> GetAllStocksAsync(SortingQuery query)
    {
        Func<IQueryable<Stock>, IOrderedQueryable<Stock>>? orderBy = null;

        switch (query.SortBy.ToLowerInvariant())
        {
            case "tickersymbol":
                orderBy = q => query.SortDirection == SortOrder.Asc
                    ? q.OrderBy(s => s.TickerSymbol)
                    : q.OrderByDescending(s => s.TickerSymbol);
                break;

            case "companyname":
                orderBy = q => query.SortDirection == SortOrder.Asc
                    ? q.OrderBy(s => s.CompanyName)
                    : q.OrderByDescending(s => s.CompanyName);
                break;

            default:
                orderBy = q => q.OrderBy(s => s.Id);
                break;
        }

        // Call the new repository method with the ordering logic
        var stocks = await _unitOfWork.StockRepository.GetAllAsync(orderBy);

        return _mapper.Map<List<Stock>>(stocks);
    }
}