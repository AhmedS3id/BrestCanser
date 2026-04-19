using BrestCanser.Api.Settings;

namespace BrestCanser.Api.Clients.MLModel.Contracts.Images.common;
public class FileSizeValidator : AbstractValidator<IFormFile>
{
	public FileSizeValidator()
	{
		RuleFor(x => x)
			.Must((request, context) => request.Length <= FileSettings.MaxFileSizeInBytes)
			.WithMessage($"Max file size is {FileSettings.MaxFileSizeInMB} MB.")
			.When(x => x is not null);
	}
}