using liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Configuration
{
    internal class ReportLikeConfiguration : IEntityTypeConfiguration<ReportLikeDbEntity>
    {
        public void Configure(EntityTypeBuilder<ReportLikeDbEntity> builder)
        {
            builder.ToTable("ReportLikes");
            builder.HasKey(like => new { like.UserId, like.ReportId });
            builder.Property(like => like.CreatedAt).IsRequired();
            builder.HasOne<UserDbEntity>()
                .WithMany()
                .HasForeignKey(like => like.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
