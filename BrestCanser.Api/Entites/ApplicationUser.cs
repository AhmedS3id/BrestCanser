namespace BrestCanser.Api.Entites;

public sealed class ApplicationUser : IdentityUser
{
    public String FirstName { get; set; } = string.Empty;
    public String LastName { get; set; } = string.Empty;
    public DateTime DateOFBirth { get; set; }

	public List<RefreshToken> RefreshTokens { get; set; } = [];
}