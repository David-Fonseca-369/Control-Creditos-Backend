using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Seguimiento_Backend.DTOs.Roles;
using Sistema_Control_Seguimiento_Backend.Entities;

namespace Sistema_Control_Seguimiento_Backend.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RolesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet("rolesSelector")]
        public async Task<ActionResult<List<RolSelectorDTO>>> RolesSelector()
        {
            return mapper.Map<List<RolSelectorDTO>>(await context.Roles.ToListAsync());
        }
    }
}
