using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Mapping
{
    public class ArchivosCursoMap : IEntityTypeConfiguration<ArchivosCurso>
    {
        public void Configure(EntityTypeBuilder<ArchivosCurso> builder)
        {
            builder.ToTable("archivosCurso")
                .HasKey(e => e.Id);

            builder.HasOne(x => x.Curso).WithMany().HasForeignKey(x => x.IdCurso);
        }
    }
}
