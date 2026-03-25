namespace BrestCanser.Api.Contracts.Authentication;

public record AuthorResponse(
    string Id,
    string? Email,
    string? PhoneNumber,
    string FirstName,
    string LastName,
    Gender Gender,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);
