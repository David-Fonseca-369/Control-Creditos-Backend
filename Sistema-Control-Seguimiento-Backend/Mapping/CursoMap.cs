using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Mapping
{
    public class CursoMap : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("curso")
                .HasKey(e => e.Id);

            builder.HasOne(x => x.CreadoPor).WithMany().HasForeignKey(x => x.IdCreadoPor);
        }
    }
}
