using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Builet.BaseRepository;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Inventory.Inventory, Inventory.InventoryDto>()
            .ForMember(dest => dest.TickerSymbol, opt => opt.MapFrom(
                src => src.Stock.TickerSymbol));

        CreateMap<Stock.CreateStockDto, Stock.Stock>();
        CreateMap<Wallet.Wallet, Wallet.WalletDto>();
        CreateMap<User.CreateUserDto, User.User>();
        CreateMap<User.User, User.UserDto>();

        CreateMap<Transaction.CreateTransactionDto, Transaction.Transaction>();
        CreateMap<Transaction.Transaction, Transaction.TransactionDto>()
            .ForMember(dest => dest.SellerUsername, opt => opt.MapFrom(
                src => src.Seller.Username))
            .ForMember(dest => dest.BuyerUsername, opt => opt.MapFrom(
                src => src.Buyer.Username));
    }
}