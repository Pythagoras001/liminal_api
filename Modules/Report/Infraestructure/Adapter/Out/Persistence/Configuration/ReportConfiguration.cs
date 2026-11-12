using liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Configuration
{
    internal class ReportConfiguration : IEntityTypeConfiguration<ReportDbEntity>
    {
        public void Configure(EntityTypeBuilder<ReportDbEntity> builder)
        {
            builder.ToTable("Reports");
            builder.HasKey(report => report.Id);
            builder.Property(report => report.Id).ValueGeneratedOnAdd();
            builder.Property(report => report.Title).IsRequired();
            builder.Property(report => report.Nivel).IsRequired();
            builder.Property(report => report.Description).IsRequired();
            builder.Property(report => report.CreatedAt).IsRequired();
            builder.Property(report => report.PrincipalEvidenceId).IsRequired().HasMaxLength(50);

            builder.HasOne<UserDbEntity>()
                .WithMany()
                .HasForeignKey(report => report.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(report => report.LevelClass)
                .WithMany()
                .HasForeignKey(report => report.LevelClassId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(report => report.PrincipalEvidence)
                .WithOne()
                .HasForeignKey<ReportDbEntity>(report => report.PrincipalEvidenceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(report => report.GaleryEvidences)
                .WithOne(evidence => evidence.GaleryReport)
                .HasForeignKey(evidence => evidence.GaleryReportId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(report => report.Likes)
                .WithOne()
                .HasForeignKey(like => like.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
