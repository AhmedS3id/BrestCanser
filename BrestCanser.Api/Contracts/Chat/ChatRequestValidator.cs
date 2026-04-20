namespace BrestCanser.Api.Contracts.Chat;


public class ChatRequestValidator : AbstractValidator<ChatRequest>
{
	public ChatRequestValidator()
	{
		RuleFor(x => x.Prompt)
					.NotEmpty()
					.Length(3, 500);
	}
}