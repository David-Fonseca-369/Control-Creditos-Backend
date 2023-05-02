using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public int IdRol { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoMaterno { get; set; }
        [MaxLength(10)]
        public string Telefono { get; set; }
        [Required, MaxLength(60), EmailAddress]
        public string Correo { get; set; }
        [MaxLength(255)]
        public string Direccion { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public bool Estado { get; set; }

        //referencias
        public Rol Rol { get; set; }
    }
}
