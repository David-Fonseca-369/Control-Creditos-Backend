using AutoMapper;
using Sistema_Control_Seguimiento_Backend.DTOs.Solicitudes;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Profiles
{
    public class SolicitudProfile : Profile
    {
        public SolicitudProfile()
        {
            CreateMap<Solicitud, SolicitudDTO>()
                .ForMember(x => x.NombreCurso, x => x.MapFrom(c => c.Curso.Nombre))
                .ForMember(x => x.NombreInstructor, x => x.MapFrom(c => $"{c.Curso.CreadoPor.Nombre} {c.Curso.CreadoPor.ApellidoPaterno} {c.Curso.CreadoPor.ApellidoMaterno}"));

            CreateMap<Solicitud, SolicitudInstructorDTO>()
                .ForMember(x => x.NombreCurso, x => x.MapFrom(c => c.Curso.Nombre))
                .ForMember(x => x.NombreAlumno, x => x.MapFrom(c => $"{c.Alumno.Nombre} {c.Alumno.ApellidoPaterno} {c.Alumno.ApellidoMaterno}"))
                .ForMember(x => x.CorreoAlumno, x => x.MapFrom(c => c.Alumno.Correo))
                .ForMember(x => x.NoCuentaAlumno, x => x.MapFrom(c => c.Alumno.NoCuenta));


        }

    }
}
