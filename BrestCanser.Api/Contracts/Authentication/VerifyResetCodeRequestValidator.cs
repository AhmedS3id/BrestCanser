namespace BrestCanser.Api.Contracts.Authentication;

public class VerifyResetCodeRequestValidator : AbstractValidator<VerifyResetCodeRequest>
{
	public VerifyResetCodeRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();

		RuleFor(x => x.Code)
			.NotEmpty()
			.MaximumLength(5);
	}
}