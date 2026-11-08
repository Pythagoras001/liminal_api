using liminal_api.Modules.User.Domain;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserDbEntity>
    {
        public void Configure(EntityTypeBuilder<UserDbEntity> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(UserDataPatters.EMAIL_MAX_LENGTH);

            builder.Property(u => u.Password)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.ImgProfileId)
                   .HasMaxLength(50);

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();

            // Relación: un usuario tiene (opcionalmente) una imagen de perfil.
            // Si se borra la imagen, el usuario queda sin foto (ImgProfileId = null).
            builder.HasOne(u => u.ImgProfile)
                   .WithMany()
                   .HasForeignKey(u => u.ImgProfileId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
