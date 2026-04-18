namespace BrestCanser.Api.Services;

public interface IAuthService
{
    Task<Result<AuthorResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthorResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result<AuthorResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<Result> SendResetPasswordCodeAsync(string email);
    Task<Result> VerifyResetCodeAsync(string email, string code);
    Task<Result> ResetPasswordAsync(string email, string code, string newPassword);
}
