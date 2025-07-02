using Builet.BaseRepository;
using Builet.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Builet.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = (await _unitOfWork.UserRepository
                .FindAsync(u => u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail))
            .FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized(new { error = "Invalid Credentials" });
        }
    
        var accessToken = _tokenService.CreateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
    
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenValidityInDays"]));
        
        _unitOfWork.UserRepository.Update(user);

        await _unitOfWork.SaveAsync();

        return Ok(new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        if (tokenDto is null)
        {
            return BadRequest("Invalid client request");
        }

        string accessToken = tokenDto.AccessToken;
        string refreshToken = tokenDto.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return BadRequest("Invalid access token.");
        }
        
        var username = principal.FindFirst("unique_name")?.Value;

        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Could not retrieve username from token.");
        }

        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Could not retrieve username from token.");
        } 
        
        var user = (await _unitOfWork.UserRepository.FindAsync(u => u.Username == username)).FirstOrDefault();

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid client request. User not found, or refresh token is invalid/expired.");
        }

        var newAccessToken = _tokenService.CreateToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        await _unitOfWork.SaveAsync();

        return Ok(new TokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
    
}