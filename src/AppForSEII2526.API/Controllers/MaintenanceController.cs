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

            return Ok(ordered);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MaintenanceDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateMaintenance(MaintenanceCreateDTO maintenanceCreate)
        {
            // Validación básica: Comprobar que el ClientId no esté vacío
            if (string.IsNullOrWhiteSpace(maintenanceCreate.ClientId))
            {
                ModelState.AddModelError("ClientId", "El ID del cliente es obligatorio.");
            }

            // Validar el método de pago (aunque no tiene sentido validarlo numéricamente, tal vez se pueda verificar el tipo de pago)
            if (string.IsNullOrWhiteSpace(maintenanceCreate.PaymentMethod))
            {
                ModelState.AddModelError("PaymentMethod", "El método de pago es obligatorio.");
            }

            // Validar la fecha: debe ser una fecha válida (esto depende de cómo lo implementes, aquí solo valida si es una fecha válida)
            if (maintenanceCreate.Date == DateTime.MinValue)
            {
                ModelState.AddModelError("Date", "La fecha no es válida.");
            }

            // Validación de los items de mantenimiento: al menos un item debe existir
            if (maintenanceCreate.MaintenanceItems == null || maintenanceCreate.MaintenanceItems.Count == 0)
            {
                ModelState.AddModelError("MaintenanceItems", "Debe incluir al menos un mantenimiento.");
            }

            // Si hay errores de validación, devolver el estado BadRequest
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Crear la entidad de mantenimiento (esto va a depender de cómo lo implementes)
            var maintenance = new Maintenance
            {
                Name = "Mantenimiento creado", // Aquí necesitarías una lógica para asignar un nombre si fuera necesario
                Price = 0m, // Puedes poner un valor por defecto o hacerlo con la lógica que prefieras
                NumberOfDays = 1, // De igual forma, poner un valor por defecto o definirlo de alguna manera
                MaintenanceTypes = new List<MaintenanceType>() // Aquí se puede agregar la lista de tipos de mantenimiento si es necesario
            };

            // Verificar que los tipos de mantenimiento proporcionados sean válidos
            foreach (var item in maintenanceCreate.MaintenanceItems)
            {
                // Aquí se busca cada tipo de mantenimiento
                var maintenanceType = await _context.MaintenanceTypes.FindAsync(item.MaintenanceId);
                if (maintenanceType == null)
                {
                    ModelState.AddModelError("MaintenanceItems", $"El tipo de mantenimiento con ID {item.MaintenanceId} no existe.");
                    continue;
                }
                // Añadir el tipo de mantenimiento a la entidad
                maintenance.MaintenanceTypes.Add(maintenanceType);
            }

            // Si hay errores en la validación de los items, devolver BadRequest
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Intentar guardar el mantenimiento en la base de datos
            try
            {
                _context.Maintenance.Add(maintenance);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando el mantenimiento.");
                return Conflict("Error al guardar el mantenimiento. Inténtalo más tarde.");
            }

            // Crear el DTO de respuesta
            // Crear el DTO de respuesta
            // Aquí el PaymentMethod viene de `maintenanceCreate`, que es el DTO
            var maintenanceDetailDTO = new MaintenanceDetailDTO(
                maintenance.Id,
                maintenanceCreate.ClientId,  // Cambié `maintenance.ClientId` por `maintenanceCreate.ClientId`
                maintenanceCreate.PaymentMethod,  // Accedemos a PaymentMethod del DTO
                maintenanceCreate.Date,
                maintenance.Price, // Esto lo tienes que calcular o pasar de otro lugar si es necesario
                maintenance.MaintenanceTypes.Select(item => new MaintenanceItemDTO(
                    item.Maintenance.Id,
                    item.Maintenance.Name ?? "Desconocido",
                    item.Type,
                    item.Maintenance.Price,
                    item.Maintenance.NumberOfDays,
                    item.Maintenance.Comment
                )).ToList()
            );


            // Retornar el DTO con el mantenimiento creado
            return CreatedAtAction(nameof(GetMaintenances), new { id = maintenance.Id }, maintenanceDetailDTO);

        }



    }
}
