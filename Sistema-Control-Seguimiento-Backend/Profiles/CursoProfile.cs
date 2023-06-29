using AutoMapper;
using Sistema_Control_Seguimiento_Backend.DTOs.ArchivosCursos;
using Sistema_Control_Seguimiento_Backend.DTOs.Cursos;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Profiles
{
    public class CursoProfile : Profile
    {
        public CursoProfile()
        {
            CreateMap<CursoCreacionDTO, Curso>();
            CreateMap<Curso, CursoDTO>();
            CreateMap<Curso, CursoPreviewDTO>();
            CreateMap<Curso, CursoPublicoDTO>()
                .ForMember(x => x.NombreInstructor, x => x.MapFrom(c => $"{c.CreadoPor.Nombre} {c.CreadoPor.ApellidoPaterno} {c.CreadoPor.ApellidoMaterno}"));
                
            CreateMap<ArchivosCurso, ArchivoCursoDTO>();
        }
    }
}
