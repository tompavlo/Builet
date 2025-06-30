using AutoMapper;
using Builet.BaseRepository;

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

    public async Task<List<Inventory>> GetAllStocksOfUserAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userId);
        if (user == null) throw new Exception("No such user");

        return await _unitOfWork.InventoryRepository.FindAsync(inv => inv.UserId == userId);
    }
    
    
}