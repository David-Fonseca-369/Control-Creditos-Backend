namespace Sistema_Control_Seguimiento_Backend.Helpers
{
    public interface IAlmacenadorArchivos
    {
        Task BorrarArchivoAsync(string ruta, string contenedor);
        Task<string> EditarArchivoAsync(string contenedor, IFormFile archivo, string ruta);
        Task<string> GuardarArchivoAsync(string contenedor, IFormFile archivo);
    }
}
