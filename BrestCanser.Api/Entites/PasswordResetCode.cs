namespace BrestCanser.Api.Entites;
public class PasswordResetCode
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
    public string CodeHash { get; set; } = default!;
    public string IdentityToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Used { get; set; }
    public int Attempts { get; set; } = 0;
}
