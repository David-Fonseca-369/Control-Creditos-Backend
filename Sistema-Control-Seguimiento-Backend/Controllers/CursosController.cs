using Microsoft.AspNetCore.Mvc;

namespace Sistema_Control_Seguimiento_Backend.Controllers
{
    [Route("api/cursos")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CursosController( ApplicationDbContext context)
        {
            this.context = context;
        }


    }
}
