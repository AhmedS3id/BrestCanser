using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrestCanser.Api.Persistance.EntitiesConfigurations;

public class PasswordResetCodeConfigurations : IEntityTypeConfiguration<PasswordResetCode>
{
    public void Configure(EntityTypeBuilder<PasswordResetCode> builder)
    {
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CodeHash).IsRequired().HasMaxLength(256);
        builder.Property(x => x.IdentityToken).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.Used).IsRequired();
    }
}