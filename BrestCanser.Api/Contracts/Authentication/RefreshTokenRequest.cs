namespace BrestCanser.Api.Contracts.Authentication;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);