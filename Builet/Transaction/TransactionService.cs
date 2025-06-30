using AutoMapper;
using Builet.BaseRepository;
using Builet.User;

namespace Builet.Transaction;

public class TransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> ShowTypeOfTransactionAsync(TransactionStatus status)
    {
        var transactions = await _unitOfWork.TransactionRepository
            .FindAsync(trans => trans.Status == status);

        return _mapper.Map<List<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> BuyTransaction(TransactionDto dto)
    {
        if (dto.BuyerId == null)
            throw new Exception("BuyerId is required");

        var buyer = await _unitOfWork.UserRepository.GetAsync(dto.BuyerId.Value);

        if (buyer == null) throw new Exception("No such buyer");

        var transaction = await _unitOfWork.TransactionRepository.GetAsync(dto.Id);

        if (transaction == null) throw new Exception("No such transaction");

        if (transaction.Quantity < dto.Quantity) throw new Exception("Too much stocks wanted to buy");

        var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(buyer.Id);
        if (dto.Quantity * dto.PricePerUnit > wallet.Balance) throw new Exception("Not enough money");
        var updatedInventory = new Inventory.Inventory
        {
            Quantity = dto.Quantity,
            Stock = await _unitOfWork.StockRepository.GetAsync(dto.StockId),
            StockId = dto.StockId,
            User = buyer,
            UserId = dto.BuyerId.Value
        };

        wallet.Balance -= dto.Quantity * dto.PricePerUnit;

        transaction.Quantity -= dto.Quantity;

        if (transaction.Quantity == 0) transaction.Status = TransactionStatus.Successful;
        
        _unitOfWork.WalletRepository.Update(wallet);
        _unitOfWork.TransactionRepository.Update(transaction);
        await _unitOfWork.InventoryRepository.AddAsync(updatedInventory);

        await _unitOfWork.SaveAsync();

        return _mapper.Map<TransactionDto>(transaction);

    }
    
    public async Task DeleteTransactionAsync(long transactionId, Guid currentUserId, Role currentUserRole)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetAsync(transactionId);
    
        if (transaction == null)
            throw new Exception($"Transaction with ID {transactionId} not found");
        
        if (currentUserRole != Role.Admin && transaction.SellerId != currentUserId)
            throw new Exception("You do not have permission to delete this transaction");

        if (transaction.Status == TransactionStatus.Successful)
            throw new Exception("You cannot delete a completed transaction");

        _unitOfWork.TransactionRepository.Remove(transaction);
        await _unitOfWork.SaveAsync();
    }

    public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto)
    {
        var seller = await _unitOfWork.UserRepository.GetAsync(dto.SellerId);
        if (seller == null) throw new Exception("There is no such seller");

        var stock = await _unitOfWork.StockRepository.GetAsync(dto.StockId);
        if (stock == null) throw new Exception("There is no such stock");

        if (dto.Quantity <= 0) throw new Exception("Quantity should be higher than 0");
        if (dto.PricePerUnit <= 0) throw new Exception("Price should be higher than 0");

        var transaction = new Transaction
        {
            SellerId = dto.SellerId,
            Seller = seller,
            StockId = dto.StockId,
            Stock = stock,
            Quantity = dto.Quantity,
            PricePerUnit = dto.PricePerUnit,
            Status = TransactionStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.TransactionRepository.AddAsync(transaction);
        return _mapper.Map<TransactionDto>(transaction);
    }
}