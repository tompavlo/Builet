using AutoMapper;
using Builet.Authentication;
using Builet.Stock;
using Microsoft.Win32.SafeHandles;

namespace Builet.User;
using Builet.BaseRepository;
using BCrypt.Net;

public class UserService 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var existingUser = await _unitOfWork.UserRepository
            .FindAsync(u => u.Username == dto.Username || u.Email == dto.Email);

        if (existingUser.Any()) throw new Exception("User with this username or email already exists.");

        var user = _mapper.Map<User>(dto);
        user.Id = Guid.NewGuid();
        user.Role = Role.User;

        user.PasswordHash = BCrypt.HashPassword(dto.Password);

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();

        var wallet = new Wallet.Wallet
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Balance = 0
        };

        await _unitOfWork.WalletRepository.AddAsync(wallet);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserDto>(user);
    } 
    
    public async Task<User> LoginAsync(LoginDto loginDto)
    {
        var user = (await _unitOfWork.UserRepository
                .FindAsync(u => u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail))
            .FirstOrDefault();

        if (user == null || !BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new Exception("Invalid username/email or password");
        }

        return user;
    }
    public async Task<UserDto> GetUserByIdentifierAsync(string identifier)
    {
        var user = (await _unitOfWork.UserRepository
                .FindAsync(u => u.Username == identifier || u.Email == identifier))
            .FirstOrDefault();

        if (user == null)
            throw new Exception($"User with identifier '{identifier}' not found");

        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(id);
        if (user == null)
            throw new Exception("User not found");

        return _mapper.Map<UserDto>(user);
    }

}