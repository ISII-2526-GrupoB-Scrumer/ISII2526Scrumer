using AppForSEII2526.API.DTOs.MaintenanceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        // Paso 2 (Select): mostrar mantenimientos disponibles
        // Flujo alternativo al paso 2: filtro por nombre y tipo
        // ===============================================================
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MaintenanceSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetMaintenances(string? nombre, string? tipo)
        {
            
            var query = await _context.Maintenance
                .Include(m => m.MaintenanceTypes)
                .Where(m =>
                    (nombre == null || m.Name.Contains(nombre)) &&
                    (tipo == null || m.MaintenanceTypes.Any(t => t.Type.Contains(tipo)))
                )
                .Select(m => new MaintenanceSelectDTO(
                    m.Id,
                    m.Name,
                    m.MaintenanceTypes.Select(t => t.Type).FirstOrDefault() ?? "Sin tipo",
                    m.Price,
                    m.NumberOfDays
                ))
                .ToListAsync();    

            
            var ordered = query
                .OrderBy(m => m.Name)
                .ThenBy(m => m.Price) 
                .ToList();
            _logger.LogInformation("Mostrando mantenimientos.");

            return Ok(ordered);
        }

    }
}
