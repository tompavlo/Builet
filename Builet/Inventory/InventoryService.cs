using AutoMapper;
using Builet.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Builet.Inventory;

public class InventoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InventoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<InventoryDto>> GetAllStocksOfUserAsync(Guid userId, PaginationQuery paginationQuery)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userId);
        if (user == null) throw new Exception("No such user found");
        
        Expression<Func<Inventory, bool>> predicate = inv => inv.UserId == userId;

 
        var totalCount = await _unitOfWork.InventoryRepository.CountAsync(predicate);
        
        var inventoryEntities = await _unitOfWork.InventoryRepository.GetPagedAsync(
            paginationQuery.PageNumber,
            paginationQuery.PageSize,
            predicate,
            include: query => query.Include(inv => inv.Stock)
        );
        
        var inventoryDtos = _mapper.Map<List<InventoryDto>>(inventoryEntities);
        
        return new PagedResult<InventoryDto>(
            inventoryDtos,
            paginationQuery.PageNumber,
            paginationQuery.PageSize,
            totalCount
        );
    }
    
}