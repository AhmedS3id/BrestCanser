using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrestCanser.Api.Persistance.EntitiesConfigurations;


public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> builder)
	{
		builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Message).IsRequired().HasMaxLength(2000);
	}
}