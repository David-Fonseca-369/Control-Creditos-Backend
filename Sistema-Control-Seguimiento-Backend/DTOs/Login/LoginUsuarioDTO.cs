using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.DTOs.Login
{
    public class LoginUsuarioDTO
    {
        [Required, EmailAddress]
        public string Correo { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
