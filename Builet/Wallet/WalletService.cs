using AutoMapper;
using Builet.BaseRepository;

namespace Builet.Wallet;

public class WalletService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WalletDto> GetWalletByUserIdAsync(Guid userId)
    {
        var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
        if (wallet == null) throw new Exception("Wallet not found");

        return _mapper.Map<WalletDto>(wallet);
    }

    public async Task AddFundsSync(Guid userId, decimal amount)
    {
        var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
        if (wallet == null) throw new Exception("Wallet not found");

        wallet.Balance += amount;

        await _unitOfWork.SaveAsync();
    }

    public async Task WithdrawFundsAsync(Guid userId, decimal amount)
    {
        var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
        if (wallet == null) throw new Exception("Wallet not found");
        if (wallet.Balance < 0) throw new Exception("Balance if not sufficient enough");

        wallet.Balance -= amount;

        await _unitOfWork.SaveAsync();
    }

}