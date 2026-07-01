using System.Text.Json.Serialization;

namespace BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
public record ProbabilitiesResponse(
	[property: JsonPropertyName("benign")] double Benign,
	[property: JsonPropertyName("malignant")] double Malignant,
	[property: JsonPropertyName("normal")] double Normal
);