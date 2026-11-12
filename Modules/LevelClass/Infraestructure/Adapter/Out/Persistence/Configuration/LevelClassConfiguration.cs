using liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Configuration
{
    internal class LevelClassConfiguration : IEntityTypeConfiguration<LevelClassDbEntity>
    {
        public void Configure(EntityTypeBuilder<LevelClassDbEntity> builder)
        {
            builder.ToTable("SurvivalClasses");
            builder.HasKey(levelClass => levelClass.Id);
            builder.Property(levelClass => levelClass.Id).ValueGeneratedOnAdd();

            builder.Property(levelClass => levelClass.Type).IsRequired();
            builder.Property(levelClass => levelClass.ClassNumber);
            builder.Property(levelClass => levelClass.SecurityLevel).IsRequired();
            builder.Property(levelClass => levelClass.Legitimacy).IsRequired();
            builder.Property(levelClass => levelClass.DangerLevel).IsRequired();
            builder.Property(levelClass => levelClass.IconImageId).IsRequired().HasMaxLength(50);
            builder.Property(levelClass => levelClass.Description).IsRequired();

            builder.HasIndex(levelClass => levelClass.IconImageId).IsUnique();
            builder.HasOne(levelClass => levelClass.IconImage)
                .WithOne()
                .HasForeignKey<LevelClassDbEntity>(levelClass => levelClass.IconImageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
