using AutoMapper;
using Sistema_Control_Seguimiento_Backend.DTOs.Usuarios;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<UsuarioCreacionDTO, Usuario>();
        }
    }
}
