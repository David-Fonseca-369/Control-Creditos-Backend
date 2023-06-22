using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sistema_Control_Seguimiento_Backend.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {

            //Registrarla en el registro de errores
            logger.LogError(context.Exception, context.Exception.Message);
            logger.LogError(context.Exception, context.Exception.StackTrace);
            var innerException = context.Exception.InnerException;

            //Se crea un object y se asegura que se devolverá esta información de la excepción al frontend como una respuesta HTTP
            context.Result = new ObjectResult(new
            {
                message = "An error occurred while processing the request.",
                error = context.Exception.Message,
                innerException = context.Exception.InnerException,
            })

            {
                // Código de estado HTTP para indicar un error interno del servidor.
                StatusCode = 500
            };

            base.OnException(context);

        }
    }
}
