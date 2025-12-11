using AppForSEII2526;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526
{

    // Controlador encargado de gestionar operaciones relacionadas con alquileres (Rentals)
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        // Dependencias inyectadas: contexto de base de datos y logger para registrar eventos
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RentalController> _logger;


        public RentalController(ApplicationDbContext context, ILogger<RentalController> logger)
        {
            _context = context;
            _logger = logger;
        }


        //PASO 7 -> Obtener un alquiler por su ID
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id)
        {
            

            // Validación inicial -> si la tabla Rental no existe en el contexto
            if (_context.Rental == null)
            {
                _logger.LogError("Error: Rentals table does not exist");
                return NotFound();
            }

            if (id < 0)
            {
                return NotFound();
            }


            // Consulta -> busca el alquiler con el ID recibido e incluye datos relacionados
            var rental = await _context.Rental
                .Where(r => r.Id == id)                 
                .Include(r => r.RentalItems)            // Incluye los ítems del alquiler
                    .ThenInclude(ri => ri.Car)          // Incluye la información del coche
                        .ThenInclude(c => c.Model)      // Incluye el modelo del coche
                .Select(r => new RentalDetailDTO(
                    r.Id,
                    r.Client.Id,
                    r.Client.Name,
                    r.Client.Surname,
                    r.Client.ClientAddress,
                    r.PaymentMethod,
                    r.RentingDate,
                    r.StartDate,
                    r.EndDate,
                    r.TotalPrice,
                    r.DeliveryCarDealer,
                    r.RentalItems.Select(ri => new RentalItemDTO(
                        ri.Car.Id,
                        ri.Car.Model.Name,
                        ri.Car.RentingPrice,
                        ri.Quantity,
                        ri.Car.Manufacturer
                    )).ToList<RentalItemDTO>()
                ))
                .FirstOrDefaultAsync(); //Devuelve el primer resultado o null si no encuentra ninguno

            // Si no encuentra el alquiler -> devuelve NotFound
            if (rental == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }

            // Si lo encuentra, lo retorna con estado OK
            return Ok(rental);
        }


        //PASO 5
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRental([FromBody] RentalCreateDTO rentalForCreate)
        {
            // ===== VALIDACIONES BÁSICAS =====
            if (rentalForCreate.StartDate <= DateTime.Today)
                ModelState.AddModelError("StartDate", "La fecha de inicio debe ser posterior a hoy.");

            if (rentalForCreate.StartDate >= rentalForCreate.EndDate)
                ModelState.AddModelError("StartDate&EndDate", "La fecha de fin debe ser posterior a la de inicio.");

            if (rentalForCreate.RentalItems == null || rentalForCreate.RentalItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Debes incluir al menos un coche para alquilar.");

            // Buscar el usuario por ClientId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == rentalForCreate.ClientId);

            if (user == null)
                ModelState.AddModelError("ClientId", "El usuario especificado no existe.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));


            // ===== COMPROBAR DISPONIBILIDAD =====
            var carIds = rentalForCreate.RentalItems.Select(i => i.CarId).Distinct().ToList();

            var cars = await _context.Car
                .Include(c => c.Model)
                .Include(c => c.RentalItems)
                    .ThenInclude(ri => ri.Rental)
                .Where(c => carIds.Contains(c.Id))
                .Select(c => new
                {
                    Car = c,
                    RentedQty = c.RentalItems
                        .Where(ri => ri.Rental.StartDate <= rentalForCreate.EndDate
                                     && ri.Rental.EndDate >= rentalForCreate.StartDate)
                        .Sum(ri => ri.Quantity)
                })
                .ToListAsync();

            foreach (var item in rentalForCreate.RentalItems)
            {
                var entry = cars.FirstOrDefault(x => x.Car.Id == item.CarId);
                if (entry == null)
                {
                    ModelState.AddModelError("RentalItems", $"El coche con Id={item.CarId} no existe.");
                    continue;
                }

                var disponible = entry.Car.QuantityForRenting - entry.RentedQty;
                if (item.Quantity <= 0)
                    ModelState.AddModelError("RentalItems", $"La cantidad para {entry.Car.Model.Name} debe ser mayor que 0.");
                else if (disponible < item.Quantity)
                    ModelState.AddModelError("RentalItems", $"No hay disponibilidad suficiente de {entry.Car.Model.Name}.");
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));


            // ===== CALCULAR PRECIO TOTAL =====
            var numDays = (rentalForCreate.EndDate - rentalForCreate.StartDate).TotalDays;
            if (numDays < 1) numDays = 1;

            decimal total = 0m;
            var rentalItems = new List<RentalItem>();

            foreach (var item in rentalForCreate.RentalItems)
            {
                var car = cars.First(c => c.Car.Id == item.CarId).Car;
                total += car.RentingPrice * item.Quantity * (decimal)numDays;

                rentalItems.Add(new RentalItem
                {
                    Car = car,
                    Quantity = item.Quantity
                });
            }

            // ===== CREAR EL OBJETO RENTAL =====
            var rental = new Rental
            {
                Client = user,
                StartDate = rentalForCreate.StartDate,
                EndDate = rentalForCreate.EndDate,
                RentingDate = DateTime.Now,
                TotalPrice = total,
                PaymentMethod = rentalForCreate.PaymentMethod,
                DeliveryCarDealer = rentalForCreate.DeliveryCarDealer,
                RentalItems = rentalItems
            };

            _context.Rental.Add(rental);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el alquiler.");
                return Conflict("Error al guardar el alquiler. Inténtalo de nuevo más tarde.");
            }

            // ===== CONSTRUIR DTO DE RESPUESTA =====
            var rentalItemsDTO = rentalItems.Select(ri =>
            {
                var car = cars.First(c => c.Car.Id == ri.Car.Id).Car;
                return new RentalItemDTO(
                    car.Id,
                    car.Model.Name,
                    car.RentingPrice,
                    ri.Quantity,
                    car.Manufacturer
                );
            }).ToList();

            var detailDTO = new RentalDetailDTO(
                rental.Id,
                rental.Client.Id,
                rental.Client.Name,
                rental.Client.Surname,
                rental.Client.ClientAddress,
                rental.PaymentMethod,
                rental.RentingDate,
                rental.StartDate,
                rental.EndDate,
                rental.TotalPrice, 
                rental.DeliveryCarDealer,
                rentalItemsDTO
            );

            return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, detailDTO);
        }





    }
}
