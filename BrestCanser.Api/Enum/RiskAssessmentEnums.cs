namespace BrestCanser.Api.Enum;

public enum AgeGroup
{
	Under30,
	From30To39,
	From40To50,
	Above50
}

public enum Ethnicity
{
	ArabCaucasian,
	African,
	Asian,
	Other
}

public enum BmiCategory
{
	Normal,
	Overweight,
	Obese
}

public enum MenarcheAge
{
	Before12,
	Between12And14,
	After14
}

public enum PregnancyHistory
{
	FirstChildBefore30,
	FirstChildAfter30,
	NeverPregnant
}

public enum MenopauseStatus
{
	NotYet,
	YesWithoutHRT,
	YesWithHRT
}

public enum FamilyHistoryLevel
{
	None,
	OneRelative,
	MoreThanOne
}

public enum EarlyFamilyDiagnosis
{
	Yes,
	No
}

public enum BrcaMutation
{
	Yes,
	NoOrNotTested
}

public enum BreastDensity
{
	Yes,
	No,
	NeverHadImaging
}

public enum BiopsyResult
{
	Yes,
	NoOrBenign,
	NeverHadBiopsy
}

public enum RadiationHistory
{
	Yes,
	No
}