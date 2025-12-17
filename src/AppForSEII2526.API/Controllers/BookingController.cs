using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII2526; // Namespace donde están tus DTOs
using AppForSEII2526.API.Models; // Namespace donde están tus modelos

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

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(MaintenanceDetailDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetBooking(int id)
        {
            if (id < 0) return NotFound();

            // Usamos una proyección para evitar el error de columna errónea en BookingItem
            var bookingData = await _context.Booking
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id,
                    b.Date,
                    b.PaymentMethod,
                    User = b.Client.UserName,
                    Address = b.Client.ClientAddress,
                    Items = b.Items.Select(bi => new
                    {
                        bi.MaintenanceID,
                        MaintName = bi.Maintenance.Name,
                        MaintPrice = bi.Maintenance.Price,
                        MaintDays = bi.Maintenance.NumberOfDays,
                        bi.Comment
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (bookingData == null) return NotFound();

            var reservaItems = bookingData.Items.Select(i => new ReservaItemDTO(
                i.MaintenanceID,
                i.MaintName,
                (double)i.MaintPrice,
                i.MaintDays,
                i.Comment
            )).ToList();

            return Ok(new MaintenanceDetailDTO(
                bookingData.Id,
                bookingData.User,
                bookingData.Address,
                bookingData.PaymentMethod,
                bookingData.Date,
                reservaItems
            ));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(MaintenanceDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateBooking(MaintenanceForCreateDTO bookingCreate)
        {
            if (bookingCreate.ReservaItems == null || !bookingCreate.ReservaItems.Any())
            {
                ModelState.AddModelError("ReservaItems", "Debe seleccionar al menos un mantenimiento.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // 1. Buscamos al usuario 'carlitos_l' en la base de datos
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == bookingCreate.ApplicationUser);

            if (user == null)
            {
                return BadRequest("El usuario indicado no existe en la base de datos.");
            }

            // 2. Creamos la entidad vinculándola al objeto 'user'
            var booking = new Booking
            {
                Date = bookingCreate.Date,
                PaymentMethod = bookingCreate.PaymentMethod,
                Client = user, // ESTO EVITA EL ERROR 409
                Items = new List<BookingItem>()
            };

            foreach (var itemDto in bookingCreate.ReservaItems)
            {
                // El campo 'reservaId' del JSON es el ID del mantenimiento
                var maintenance = await _context.Maintenance.FindAsync(itemDto.ReservaId);
                if (maintenance == null) continue;

                // Validación de comentario obligatorio 
                if (string.IsNullOrWhiteSpace(itemDto.Comentarios) || itemDto.Comentarios == "null")
                {
                    ModelState.AddModelError("Comentarios", $"El comentario para {maintenance.Name} es obligatorio.");
                    continue;
                }

                booking.Items.Add(new BookingItem
                {
                    MaintenanceID = maintenance.Id,
                    Comment = itemDto.Comentarios
                });
            }

            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            try
            {
                _context.Booking.Add(booking);
                await _context.SaveChangesAsync();

                // Retornamos el DTO de detalle con el ID generado automáticamente
                var detail = new MaintenanceDetailDTO(
                    booking.Id,
                    user.UserName,
                    user.ClientAddress,
                    booking.PaymentMethod,
                    booking.Date,
                    bookingCreate.ReservaItems
                );

                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de integridad SQL");
                return Conflict("Error al procesar la reserva: comprueba que el usuario y los mantenimientos son válidos.");
            }
        }
    }
}