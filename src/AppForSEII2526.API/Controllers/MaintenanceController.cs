using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsController> _logger;


        public MaintenanceController(ApplicationDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MaintenanceSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetMaintenances(string? nombre, string? tipo)
        {
            IList<MaintenanceSelectDTO> mantenimientos = await _context.Maintenance
                .Include(m => m.MaintenanceTypes)
                .Where(m =>
                    (nombre == null || m.Name.Contains(nombre)) &&
                    (tipo == null || m.MaintenanceTypes.Any(t => t.Type.Contains(tipo)))
                )
                .OrderBy(m => m.Name)
                .ThenBy(m => m.Price)
                .Select(m => new MaintenanceSelectDTO(
                    m.Id,
                    m.Name,
                    m.MaintenanceTypes.FirstOrDefault() != null
                        ? m.MaintenanceTypes.FirstOrDefault()!.Type
                        : "Sin tipo",
                    m.Price,
                    m.NumberOfDays
                ))
                .ToListAsync();

            return Ok(mantenimientos);
        }
    }
}
