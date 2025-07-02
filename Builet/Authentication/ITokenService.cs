using System.Security.Claims;

namespace Builet.Authentication;

public interface ITokenService
{
    string CreateToken(User.User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}