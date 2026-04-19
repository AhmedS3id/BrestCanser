using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrestCanser.Api.Persistance.EntitiesConfigurations;


public class PredictionHistoryConfigurations : IEntityTypeConfiguration<PredictionHistory>
{
	public void Configure(EntityTypeBuilder<PredictionHistory> builder)
	{
		builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);
		builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(2000);
		builder.Property(x => x.Diagnosis).IsRequired().HasMaxLength(100);
		builder.Property(x => x.Confidence).IsRequired();
		builder.Property(x => x.Status).IsRequired().HasMaxLength(100);
		builder.Property(x => x.Message).IsRequired() .HasMaxLength(2000);
	}
}