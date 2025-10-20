using AppForSEII2526.API.DTOs.CarsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
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
        [ProducesResponseType(typeof(IList<CarforRental>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCoches_Datos()
        {
            var coches = await _context.Car
                .Select(c => new CarforRental(c.Id, c.Model.Name, c.FuelType, c.Manufacturer, c.RentingPrice, c.Color))
                .ToListAsync();
            return Ok(coches);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarforRental>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCoches_FILTRO_MODELO_DTO(string? modelo) { 
            
            
            IList<CarforRental> coches = await _context.Car
                .Where(c => c.Model.Name.Contains(modelo) || (modelo == null))
                .Select(c => new CarforRental(c.Id, c.Model.Name, c.FuelType, c.Manufacturer, c.RentingPrice, c.Color))
                .ToListAsync();

            return Ok(coches);
        }




        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarforRental>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCoches_Filtrados_Modelo_Precio(string? modelo, decimal? precio)
        {
            try
            {


                var query = _context.Car.AsQueryable();

                if (!string.IsNullOrWhiteSpace(modelo))
                    query = query.Where(c => c.Model.Name.Contains(modelo));


                if (precio.HasValue)
                    query = query.Where(c => c.RentingPrice <= precio.Value);


                var cochesFiltrados = await query
                    .Select(c => new CarforRental(
                        c.Id,
                        c.Model.Name,
                        c.FuelType,
                        c.Manufacturer,
                        c.RentingPrice,
                        c.Color))
                    .ToListAsync();

                if (!cochesFiltrados.Any())
                    return NotFound("No se encontraron coches que coincidan con los filtros establecidos.");

                return Ok(cochesFiltrados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al aplicar filtros de modelo y precio en los coches.");
                return StatusCode(500, "Ocurrió un error al filtrar los coches.");
            }
        }

    }
}
