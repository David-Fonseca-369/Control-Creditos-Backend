using AutoMapper;
using Sistema_Control_Seguimiento_Backend.DTOs.Roles;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Profiles
{
    public class RolProfile : Profile
    {
        public RolProfile()
        {
            CreateMap<Rol, RolPickerDTO>();
        }
    }
}
