﻿using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.DTOs.Usuarios
{
    public class UsuarioCreacionDTO
    {
        [Required]
        public int IdRol { get; set; }
        [Required, MaxLength(60)]        
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoMaterno { get; set; }
        [MinLength(10), MaxLength(10)]
        public string Telefono { get; set; }
        [Required, MaxLength(60), EmailAddress]
        public string Correo { get; set; }
        [MaxLength(255)]
        public string Direccion { get; set; }
        [Required, MaxLength(60), MinLength(8)]
        public string Password { get; set; }
      
    }
}
