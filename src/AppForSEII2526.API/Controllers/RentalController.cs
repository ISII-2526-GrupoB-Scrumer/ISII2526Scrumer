using AppForSEII2526.API.DTOs.RentalDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsController> _logger;


        public RentalController(ApplicationDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id)
        {
            if (_context.Rental == null)
            {
                _logger.LogError("Error: Rentals table does not exist");
                return NotFound();
            }

            var rental = await _context.Rental
                .Where(r => r.Id == id)
                .Include(r => r.RentalItems)
                    .ThenInclude(ri => ri.Car)
                        .ThenInclude(c => c.Model)
                .Select(r => new RentalDetailDTO(
                    r.Id,
                    r.StartDate,
                    r.EndDate,
                    r.PaymentMethod,
                    r.DeliveryCarDealer,
                    r.Client.UserName,
                    r.TotalPrice,
                    r.RentalItems.Select(ri => new RentalItemDTO(
                        ri.Car.Id,
                        ri.Car.Model.Name,
                        ri.Car.RentingPrice,
                        ri.Quantity
                    )).ToList<RentalItemDTO>()
                ))
                .FirstOrDefaultAsync();

            if (rental == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }

            return Ok(rental);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRental(RentalCreateDTO rentalForCreate)
        {
            // --- VALIDACIONES DEL CASO DE USO ---
            if (rentalForCreate.StartDate <= DateTime.Today)
                ModelState.AddModelError("StartDate", "Error: La fecha de inicio debe ser posterior a hoy.");

            if (rentalForCreate.StartDate >= rentalForCreate.EndDate)
                ModelState.AddModelError("StartDate&EndDate", "Error: La fecha de fin debe ser posterior a la de inicio.");

            if (rentalForCreate.RentalItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Error: Debe incluir al menos un coche en el alquiler.");

            var user = _context.Users.FirstOrDefault(u => u.Id == rentalForCreate.ClientId);
            if (user == null)
                ModelState.AddModelError("ClientId", "Error: El cliente no está registrado.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- COMPROBACIÓN DE DISPONIBILIDAD DE LOS COCHES ---
            var carIds = rentalForCreate.RentalItems.Select(ri => ri.CarId).ToList();

            var cars = _context.Car
                .Include(c => c.Model)
                .Where(c => carIds.Contains(c.Id))
                .Select(c => new
                {
                    c.Id,
                    c.Model.Name,
                    c.QuantityForRenting,
                    c.RentingPrice,
                    NumberOfRentedItems = _context.RentalItem
                        .Include(ri => ri.Rental)
                        .Count(ri => ri.CarId == c.Id &&
                                     ri.Rental.StartDate <= rentalForCreate.EndDate &&
                                     ri.Rental.EndDate >= rentalForCreate.StartDate)
                })
                .ToList();

            // --- CREACIÓN DEL RENTAL ---
            var rental = new Rental
            {
                Client = user,
                DeliveryCarDealer = rentalForCreate.DeliveryCarDealer,
                PaymentMethod = rentalForCreate.PaymentMethod,
                StartDate = rentalForCreate.StartDate,
                EndDate = rentalForCreate.EndDate,
                TotalPrice = 0,
                RentalItems = new List<RentalItem>()
            };

            var numDays = (rentalForCreate.EndDate - rentalForCreate.StartDate).TotalDays;

            foreach (var item in rentalForCreate.RentalItems)
            {
                var car = cars.FirstOrDefault(c => c.Id == item.CarId);
                if (car == null || car.NumberOfRentedItems >= car.QuantityForRenting)
                {
                    ModelState.AddModelError("RentalItems", $"Error: El coche con ID {item.CarId} no está disponible entre {rentalForCreate.StartDate.ToShortDateString()} y {rentalForCreate.EndDate.ToShortDateString()}.");
                }
                else
                {
                    rental.RentalItems.Add(new RentalItem
                    {
                        CarId = car.Id,
                        Rental = rental,
                        Quantity = item.Quantity
                    });
                    item.RentingPrice = car.RentingPrice;
                }
            }

            rental.TotalPrice = rental.RentalItems.Sum(ri =>
            {
                var car = cars.FirstOrDefault(c => c.Id == ri.CarId);
                return (car?.RentingPrice ?? 0) * (decimal)numDays;
            });

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- GUARDAR EN BASE DE DATOS ---
            _context.Add(rental);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", "Error al guardar el alquiler. Inténtelo más tarde.");
                return Conflict("Error: " + ex.Message);
            }

            // --- DEVOLVER DETALLE DEL RENTAL ---
            var rentalDetail = new RentalDetailDTO(
                rental.Id,
                rental.StartDate,
                rental.EndDate,
                rental.PaymentMethod,
                rental.DeliveryCarDealer,
                rental.Client.UserName,
                rental.TotalPrice,
                rentalForCreate.RentalItems
            );

            return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rentalDetail);
        }


    }
}
