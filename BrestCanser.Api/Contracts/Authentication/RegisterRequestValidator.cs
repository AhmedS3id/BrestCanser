
namespace BrestCanser.Api.Contracts.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .Length(3, 250);

        RuleFor(x => x.LastName)
                    .NotEmpty()
                    .Length(3, 250);


        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
                    .NotEmpty()
                    .Length(11)
                    .WithMessage("The length of 'Phone Number' must be 11 characters.")
                    .Matches(RegexPatterns.PhoneNumber)
                    .WithMessage("Phone number must be a valid Egyptian phone number.");

        RuleFor(x => x.Gender)
                  .IsInEnum()
                  .WithMessage("Gender must be Male or Female.");

        RuleFor(x => x.Password)
                    .NotEmpty()
                    .Matches(RegexPatterns.Password)
                    .WithMessage("Password should be at least 8 digits and should contains Lowercase, Uppercase and NonAlphanumeric");

        RuleFor(x => x.ConfirmPassword)
                 .NotEmpty()
                 .Equal(x => x.Password)
                 .WithMessage("Confirm password must be equal password.");
    }
}