namespace BrestCanser.Api.Contracts.Users;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .Length(3, 250);

        RuleFor(x => x.LastName)
                    .NotEmpty()
                    .Length(3, 250);

        RuleFor(x => x.PhoneNumber)
                    .NotEmpty()
                    .Length(11)
                    .WithMessage("The length of 'Phone Number' must be 11 characters.")
                    .Matches(RegexPatterns.PhoneNumber)
                    .WithMessage("Phone number must be a valid Egyptian phone number.");

        RuleFor(x => x.Gender)
                      .IsInEnum()
                      .WithMessage("Gender must be Male or Female.");
    }
}
