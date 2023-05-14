namespace Sistema_Control_Seguimiento_Backend.DTOs.Alumnos
{
    public class AlumnoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NoCuenta { get; set; }
        public string Correo { get; set; }
        public bool Estado { get; set; }
    }
}
