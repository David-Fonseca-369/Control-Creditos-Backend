using Microsoft.EntityFrameworkCore;

namespace Sistema_Control_Seguimiento_Backend.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabeceraAsync<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            //Contamos la cantidad de registros de la tabla
            double cantidad = await queryable.CountAsync();

            //Lo agregamos a la cabecera
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
            //exponemos la cabecera en el startup

        }

        public static void InsertarParametrosPaginacionEnCabecera(this HttpContext httpContext, int cantidad)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            //lo agrego a la cabecera
            httpContext.Request.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
        }
    }
}
