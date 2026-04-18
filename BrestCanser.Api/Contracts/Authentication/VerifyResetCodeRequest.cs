namespace BrestCanser.Api.Contracts.Authentication;

public record VerifyResetCodeRequest(
    string Email,
    string Code
);