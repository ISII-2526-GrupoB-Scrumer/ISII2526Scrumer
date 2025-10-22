using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> GetMantenimientos_Filtrados_Nombre_Tipo(string? nombre, string? tipo)
        {
            try
            {
                // 🔹 Paso 2.1: El sistema ofrece la posibilidad de filtrar por nombre y tipo
                var query =  _context.Maintenance
                    .Include(m => m.MaintenanceTypes)
                    .AsQueryable();

                // 🔹 Paso 2.2: El cliente selecciona los filtros que le interesan
                if (!string.IsNullOrWhiteSpace(nombre))
                    query = query.Where(m => m.Name.Contains(nombre));

                if (!string.IsNullOrWhiteSpace(tipo))
                    query = query.Where(m => m.MaintenanceTypes.Any(t => t.Type.Contains(tipo)));

                // 🔹 Paso 2.3: El sistema muestra los mantenimientos que cumplen los filtros
                var mantenimientosFiltrados = await query
                    .Select(m => new CarforMaintenance(
                        m.Name,
                        m.MaintenanceTypes.FirstOrDefault() != null ? m.MaintenanceTypes.FirstOrDefault()!.Type : "Sin tipo",
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
    }
}
