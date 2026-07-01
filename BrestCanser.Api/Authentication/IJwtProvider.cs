namespace BrestCanser.Api.Authentication;
public interface IJwtProvider
{
	(string token, int expiresIn) GenerateToken(ApplicationUser user);
    string? GetUserIdFromExpiredToken(string token);
    string? ValidateToken(string token);

}