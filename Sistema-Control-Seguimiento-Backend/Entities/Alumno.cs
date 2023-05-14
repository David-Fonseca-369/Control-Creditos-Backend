using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.Entities
{
    public class Alumno
    {
        public int Id { get; set; }       
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
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
