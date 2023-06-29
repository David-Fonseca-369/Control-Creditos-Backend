using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Mapping
{
    public class SolicitudMap : IEntityTypeConfiguration<Solicitud>
    {
        public void Configure(EntityTypeBuilder<Solicitud> builder)
        {
            builder.ToTable("solicitud")
                .HasKey(x => x.Id);

            builder.HasOne(x => x.Curso).WithMany().HasForeignKey(x => x.IdCurso);
            builder.HasOne(x => x.Alumno).WithMany().HasForeignKey(x => x.IdAlumno);
        }
    }
}
