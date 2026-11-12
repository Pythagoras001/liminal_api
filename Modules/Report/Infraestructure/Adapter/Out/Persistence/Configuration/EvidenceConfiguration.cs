using liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Configuration
{
    internal class EvidenceConfiguration : IEntityTypeConfiguration<EvidenceDbEntity>
    {
        public void Configure(EntityTypeBuilder<EvidenceDbEntity> builder)
        {
            builder.ToTable("Evidences");
            builder.HasKey(evidence => evidence.Id);
            builder.Property(evidence => evidence.Id).HasMaxLength(50);
            builder.Property(evidence => evidence.Description).IsRequired();
            builder.Property(evidence => evidence.ImageId).IsRequired().HasMaxLength(50);
            builder.HasIndex(evidence => evidence.ImageId).IsUnique();
            builder.HasOne(evidence => evidence.Image)
                .WithOne()
                .HasForeignKey<EvidenceDbEntity>(evidence => evidence.ImageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
