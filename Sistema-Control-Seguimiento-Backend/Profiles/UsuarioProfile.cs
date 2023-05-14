using AutoMapper;
using Sistema_Control_Seguimiento_Backend.DTOs.Usuarios;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>().ForMember(x => x.NombreRol, 
                x => x.MapFrom( r => r.Rol.Nombre));
            CreateMap<UsuarioCreacionDTO, Usuario>();
            CreateMap<UsuarioEditarDTO, Usuario>();
            CreateMap<Usuario, UsuarioPreviewDTO>();
        }
    }
}
