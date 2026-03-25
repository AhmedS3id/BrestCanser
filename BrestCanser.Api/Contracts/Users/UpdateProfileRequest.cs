namespace BrestCanser.Api.Contracts.Users;


public record UpdateProfileRequest(
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    Gender Gender
);
