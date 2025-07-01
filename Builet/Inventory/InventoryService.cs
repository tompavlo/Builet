using AutoMapper;
using Builet.BaseRepository;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<InventoryDto>> GetAllStocksOfUserAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userId);
        if (user == null) throw new Exception("No such user found");
        
        var inventoryEntities = await _unitOfWork.InventoryRepository.FindAsync(
            predicate: inv => inv.UserId == userId,
            include: query => query.Include(inv => inv.Stock)
        );
        return _mapper.Map<List<InventoryDto>>(inventoryEntities);
    }
    
}