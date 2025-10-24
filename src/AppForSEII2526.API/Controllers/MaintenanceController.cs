using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaintenanceController> _logger;

        public MaintenanceController(ApplicationDbContext context, ILogger<MaintenanceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===============================================================
        // Caso de uso 3: Contratar mantenimientos
        // Flujo alternativo 0 - al paso 2
        // El sistema ofrece filtrar por nombre y tipo de mantenimiento
        // ===============================================================

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarforMaintenance>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMantenimientos_Filtrados_Nombre_Tipo_Mantenimiento(string? nombre, string? tipo)
        {
            try
            {
                var query = _context.Maintenance
                    .Include(m => m.MaintenanceTypes)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(nombre))
                    query = query.Where(m => m.Name.Contains(nombre));

                if (!string.IsNullOrWhiteSpace(tipo))
                    query = query.Where(m => m.MaintenanceTypes.Any(t => t.Type.Contains(tipo)));

                var mantenimientosFiltrados = await query
                    .Select(m => new CarforMaintenance(
                        
                        m.Name,
                        m.MaintenanceTypes.FirstOrDefault() != null
                            ? m.MaintenanceTypes.FirstOrDefault()!.Type
                            : "Sin tipo",
                        m.Price,
                        m.NumberOfDays))
                    .ToListAsync();

                if (!mantenimientosFiltrados.Any())
                    return NotFound("No se encontraron mantenimientos con los filtros seleccionados.");

                return Ok(mantenimientosFiltrados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar mantenimientos por nombre y tipo.");
                return StatusCode(500, "Ocurrió un error al obtener los mantenimientos filtrados.");
            }
        }

        // ===============================================================
        // Método adicional: mostrar todos los mantenimientos disponibles
        // ===============================================================
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarforMaintenance>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCoches_Datos_Mantenimiento()
        {
            var query = _context.Maintenance
                .Include(m => m.MaintenanceTypes)
                .AsQueryable();

            var mantenimientos = await query
                .Select(m => new CarforMaintenance(
                    
                    m.Name,
                    m.MaintenanceTypes.FirstOrDefault() != null
                        ? m.MaintenanceTypes.FirstOrDefault()!.Type
                        : "Sin tipo",
                    m.Price,
                    m.NumberOfDays))
                .ToListAsync();

            return Ok(mantenimientos);
        }
    }
}
