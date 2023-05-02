using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario")
                .HasKey(x => x.Id);

            builder.HasOne(x => x.Rol).WithMany().HasForeignKey(x => x.IdRol);
        }
    }
}
