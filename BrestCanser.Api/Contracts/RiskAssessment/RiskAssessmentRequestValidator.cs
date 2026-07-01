namespace BrestCanser.Api.Contracts.RiskAssessment;

using FluentValidation;

public class RiskAssessmentRequestValidator : AbstractValidator<RiskAssessmentRequest>
{
	public RiskAssessmentRequestValidator()
	{
		// Stage 1: Personal Info
		RuleFor(x => x.AgeGroup)
			.IsInEnum().WithMessage("Invalid AgeGroup");

		RuleFor(x => x.Ethnicity)
			.IsInEnum().WithMessage("Invalid Ethnicity");

		RuleFor(x => x.BmiCategory)
			.IsInEnum().WithMessage("Invalid BmiCategory");

		// Stage 2: Hormonal History
		RuleFor(x => x.MenarcheAge)
			.IsInEnum().WithMessage("Invalid MenarcheAge");

		RuleFor(x => x.PregnancyHistory)
			.IsInEnum().WithMessage("Invalid PregnancyHistory");

		RuleFor(x => x.MenopauseStatus)
			.IsInEnum().WithMessage("Invalid MenopauseStatus");

		// Stage 3: Family & Genetic History
		RuleFor(x => x.FamilyHistoryLevel)
			.IsInEnum().WithMessage("Invalid FamilyHistoryLevel");

		RuleFor(x => x.EarlyFamilyDiagnosis)
			.IsInEnum().WithMessage("Invalid EarlyFamilyDiagnosis");

		RuleFor(x => x.BrcaMutation)
			.IsInEnum().WithMessage("Invalid BrcaMutation");

		// Stage 4: Medical History
		RuleFor(x => x.BreastDensity)
			.IsInEnum().WithMessage("Invalid BreastDensity");

		RuleFor(x => x.BiopsyResult)
			.IsInEnum().WithMessage("Invalid BiopsyResult");

		RuleFor(x => x.RadiationHistory)
			.IsInEnum().WithMessage("Invalid RadiationHistory");
	}
}
