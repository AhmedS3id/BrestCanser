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

        }
}
