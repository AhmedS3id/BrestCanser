using BrestCanser.Api.Clients.MLModel.Contracts.Images.common;

namespace BrestCanser.Api.Clients.MLModel.Contracts.MLModel;

public class PredictRequestValidator : AbstractValidator<PredictRequest>
{
	public PredictRequestValidator()
	{
		RuleFor(x => x.File)
			.NotNull()
			.SetValidator(new FileSizeValidator())
			.SetValidator(new FileSignatureValidator());
	}
}
