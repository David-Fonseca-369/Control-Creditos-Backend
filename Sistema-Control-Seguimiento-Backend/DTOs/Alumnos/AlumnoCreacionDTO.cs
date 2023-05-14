using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.DTOs.Alumnos
{
    public class AlumnoCreacionDTO
    {
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoMaterno { get; set; }
        [MinLength(10), MaxLength(10)]
        public string Telefono { get; set; }
        [Required, MinLength(10), MaxLength(10)]
        public string NoCuenta { get; set; }
        [Required, MaxLength(60), EmailAddress]
        public string Correo { get; set; }
        [Required, MinLength(8), MaxLength(60)]
        public string Password { get; set; }

    }
}
