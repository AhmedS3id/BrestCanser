namespace BrestCanser.Api.Options;

public sealed class RiskScoringOptions
{
	public const string SectionName = "RiskScoring";

	public int MaxScore { get; init; } = 135;
	public Dictionary<string, int> Weights { get; init; } = new();
}
