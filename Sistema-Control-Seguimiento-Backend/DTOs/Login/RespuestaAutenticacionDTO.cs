namespace Sistema_Control_Seguimiento_Backend.DTOs.Login
{
    public class RespuestaAutenticacionDTO
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
