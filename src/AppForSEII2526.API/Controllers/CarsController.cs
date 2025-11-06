using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII2526.API.Data;

namespace AppForSEII2526
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsController> _logger;


        public CarsController(ApplicationDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Car>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCars_sinDTOs()
        {
            IList<Car> coches = await _context.Car.ToListAsync();
            return Ok(coches);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaCompra(string? modelo, string? color)
        {
            IList<PurchaseSelectDTO> coches = await _context.Car
                .Include(c => c.Model)
                .Where(c =>
                    (modelo == null || c.Model.Name.Contains(modelo)) &&
                    (color == null || c.Color.Contains(color))
                )
                .OrderBy(c => c.Model.Name)
                .ThenBy(c => c.PurchasingPrice)
                .Select(c => new PurchaseSelectDTO(
                    c.Id,
                    c.Model.Name,
                    c.Manufacturer,
                    c.Color,
                    c.PurchasingPrice
                ))
                .ToListAsync();

            return Ok(coches);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaAlquilar(string? modelo, decimal? precio)
        {
            IList<RentalSelectDTO> coches = await _context.Car
                .Include(c => c.Model)
                .Where(c =>
                    (modelo == null || c.Model.Name.Contains(modelo)) &&
                    (precio == null || c.RentingPrice <= precio)
                )
                .OrderBy(c => c.Model.Name)
                .ThenBy(c => c.RentingPrice)
                .Select(c => new RentalSelectDTO(
                    c.Id,
                    c.Model.Name,
                    c.Manufacturer,
                    c.FuelType,
                    c.RentingPrice,
                    c.Color
                ))
                .ToListAsync();

            return Ok(coches);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ReviewSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaReview(string? fabricante, string? fuelType)
        {
            if (_context.Car == null || !_context.Car.Any())
            {
                var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "Error: Cars table does not exist or no cars available" } }
            });

                return BadRequest(problemDetails);
            }

            IList<ReviewSelectDTO> coches = await _context.Car
                .Include(c => c.Model)
                .Where(c =>
                    (fabricante == null || c.Manufacturer.Contains(fabricante)) &&
                    (fuelType == null || c.FuelType.Contains(fuelType))
                )
                .OrderBy(c => c.Model.Name)
                .ThenBy(c => c.Manufacturer)
                .Select(c => new ReviewSelectDTO(
                    c.Id,
                    c.Model.Name,
                    c.CarClass,
                    c.Manufacturer,
                    c.FuelType,
                    c.Color
                ))
                .ToListAsync();

            return Ok(coches);
        }


        




    }
}
