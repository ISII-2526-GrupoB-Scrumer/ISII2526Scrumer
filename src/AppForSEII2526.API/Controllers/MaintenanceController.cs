using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII2526; // Namespace de tus nuevos DTOs

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Paso 2: Listar mantenimientos con filtros
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MantenimientoDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetMaintenances(string? nombre, string? tipo)
        {
            var query = _context.Maintenance.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(m => m.Name.Contains(nombre));

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(m => m.MaintenanceTypes.Any(t => t.Type.Contains(tipo)));

            var data = await query
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    TipoPrincipal = m.MaintenanceTypes.Select(t => t.Type).FirstOrDefault() ?? "General",
                    m.Price,
                    m.NumberOfDays
                })
                .ToListAsync();

            var result = data.Select(d => new MantenimientoDTO(
                d.Id,
                d.Name,
                d.TipoPrincipal,
                (double)d.Price,
                d.NumberOfDays
            ))
            .OrderBy(m => m.Nombre)
            .ToList();

            return Ok(result);
        }

    }
}