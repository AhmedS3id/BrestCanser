namespace BrestCanser.Api.Entites;

public class PredictionHistory
{
	public int Id { get; set; }

	public string ImageUrl { get; set; } = default!;

	public string Diagnosis { get; set; } = default!;

	public double Confidence { get; set; }

	public PredictionStatus Status { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public string UserId { get; set; } = default!;

	public ApplicationUser User { get; set; } = default!; // Navigation property to the ApplicationUser entity
}