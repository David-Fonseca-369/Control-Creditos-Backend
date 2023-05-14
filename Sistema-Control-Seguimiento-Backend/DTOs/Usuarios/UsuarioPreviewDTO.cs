namespace Sistema_Control_Seguimiento_Backend.DTOs.Usuarios
{
    public class UsuarioPreviewDTO
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public bool Estado { get; set; }
    }
}
