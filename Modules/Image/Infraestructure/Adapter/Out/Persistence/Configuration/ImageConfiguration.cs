using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Configuration
{
    internal class ImageConfiguration : IEntityTypeConfiguration<ImageDbEntity>
    {
        public void Configure(EntityTypeBuilder<ImageDbEntity> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                   .HasMaxLength(50);

            builder.Property(i => i.PublicId)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(i => i.OriginalUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(i => i.ThumbnailUrl)
                   .HasMaxLength(500);

            builder.Property(i => i.MediumUrl)
                   .HasMaxLength(500);

            builder.Property(i => i.CreatedAt)
                   .IsRequired();

            builder.HasIndex(i => i.PublicId).IsUnique();
        }
    }
}
