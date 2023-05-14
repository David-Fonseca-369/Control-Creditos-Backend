using AutoMapper;
using Sistema_Control_Seguimiento_Backend.DTOs.Alumnos;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Profiles
{
    public class AlumnoProfile : Profile
    {
        public AlumnoProfile()
        {
            CreateMap<Alumno, AlumnoDTO>();
            CreateMap<AlumnoCreacionDTO, Alumno>();
            CreateMap<AlumnoEditarDTO, Alumno>();
            CreateMap<Alumno, AlumnoPreviewDTO>();
        }
    }
}
