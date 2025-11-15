using AppForSEII2526.API.DTOs.MaintenanceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(ApplicationDbContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===========================================================
        // GET Booking (Paso 7 del caso de uso)
        // Muestra el detalle de una contratación de mantenimiento
        // ===========================================================
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(MaintenanceDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBooking(int id)
        {
            if (_context.Booking == null)
            {
                _logger.LogError("Error: Booking table does not exist");
                return NotFound();
            }

            var booking = await _context.Booking
                .Where(b => b.Id == id)
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Maintenance)
                        .ThenInclude(m => m.MaintenanceTypes)
                .Include(b => b.Client)
                .Select(b => new MaintenanceDetailDTO(
                    b.Id,
                    b.Client.Id,            // o b.Client.UserName si prefieres mostrar el nombre
                    b.PaymentMethod,
                    b.Date,
                    b.Items.Sum(bi => bi.Maintenance.Price), // o b.Items.Sum(...) si quieres el total
                    b.Items.Select(bi => new MaintenanceItemDTO(
                        bi.Maintenance.Id,
                        bi.Maintenance.Name,
                        bi.Maintenance.MaintenanceTypes != null ?
                            string.Join(", ", bi.Maintenance.MaintenanceTypes.Select(mt => mt.Type)) :
                            "Desconocido",
                        bi.Maintenance.Price,
                        bi.Maintenance.NumberOfDays,
                        bi.Comment
                    )).ToList()
                ))

                .FirstOrDefaultAsync();

            if (booking == null)
            {
                _logger.LogError($"Error: Booking with id {id} does not exist");
                return NotFound();
            }

            return Ok(booking);
        }

        // ===========================================================
        // POST Booking (Paso 5 del caso de uso)
        // Crea una contratación de mantenimiento
        // ===========================================================
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MaintenanceDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateBooking(MaintenanceCreateDTO bookingCreate)
        {
            // Validaciones iniciales básicas
            if (bookingCreate.MaintenanceItems == null || bookingCreate.MaintenanceItems.Count == 0)
                ModelState.AddModelError("MaintenanceItems", "Debe seleccionar al menos un mantenimiento.");

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == bookingCreate.ClientId);

            if (user == null)
                ModelState.AddModelError("Client", "El usuario indicado no existe.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Se crean los objetos del dominio
            var booking = new Booking
            {
                Date = DateTime.Now,
                PaymentMethod = bookingCreate.PaymentMethod,
                Client = user!,
                ClientId = user!.Id,
                Items = new List<BookingItem>()
            };

            decimal total = 0;

            foreach (var item in bookingCreate.MaintenanceItems)
            {
                var maintenance = await _context.Maintenance.FindAsync(item.MaintenanceId);

                if (maintenance == null)
                {
                    ModelState.AddModelError("MaintenanceItems", $"El mantenimiento con ID {item.MaintenanceId} no existe.");
                    continue;
                }

                booking.Items.Add(new BookingItem
                {
                    MaintenanceID = maintenance.Id,
                    Comment = item.Comment,
                });

                total += maintenance.Price;
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            try
            {
                _context.Booking.Add(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando la reserva de mantenimiento.");
                return Conflict("Error al guardar la reserva. Inténtalo más tarde.");
            }

            // Devuelve el DTO con los datos creados
            var detailDTO = new MaintenanceDetailDTO(
                booking.Id,
                booking.Client.UserName,
                booking.PaymentMethod,
                booking.Date,
                booking.Items.Sum(bi => bi.Maintenance.Price),
                booking.Items.Select(bi => new MaintenanceItemDTO(
                    bi.Maintenance.Id,
                    bi.Maintenance.Name ?? "Desconocido",
                    bi.Maintenance.MaintenanceTypes != null ?
                        string.Join(", ", bi.Maintenance.MaintenanceTypes.Select(mt => mt.Type)) :
                        "Desconocido",
                    bi.Maintenance.Price,
                    bi.Maintenance.NumberOfDays,
                    bi.Comment
                )).ToList()
            );

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, detailDTO);
        }
    }
}
