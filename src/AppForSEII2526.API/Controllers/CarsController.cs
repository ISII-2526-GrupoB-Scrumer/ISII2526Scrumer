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
        public async Task<ActionResult> GetCarssinDTOs()
        {
            IList<Car> coches = await _context.Car.ToListAsync();
            return Ok(coches);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaCompra(string? modelo, string? color)
        {
            // 1. Obtenemos los coches de la BD sin la ordenación problemática
            //    (Sacamos el OrderBy y ThenBy de la consulta a la BD)
            IList<PurchaseSelectDTO> coches = await _context.Car
                .Include(c => c.Model)
                .Where(c =>
                    (modelo == null || c.Model.Name.Contains(modelo)) &&
                    (color == null || c.Color.Contains(color))
                )
                .Select(c => new PurchaseSelectDTO(
                    c.Id,
                    c.Model.Name,
                    c.Manufacturer,
                    c.Color,
                    c.PurchasingPrice
                ))
                .ToListAsync(); // <-- Traemos la lista a memoria

            // 2. Ahora ordenamos la lista en memoria (LINQ to Objects)
            //    Esto sí soporta ordenar por decimales.
            var cochesOrdenados = coches
                .OrderBy(c => c.ModelName)
                .ThenBy(c => c.PurchasingPrice)
                .ToList();

            return Ok(cochesOrdenados);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaAlquilar(string? modelo, decimal? precio)
        {

            // Si la tabla de coches no existe o no hay coches registrados, devolvemos BadRequest
            if (_context.Car == null || !_context.Car.Any())
            {
                var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "Error: Cars table does not exist or no cars available" } }
                });

                return BadRequest(problemDetails); 
            }

            // Consulta de coches aplicando filtros opcionales (modelo y/o precio),
            // incluyendo datos del modelo asociado y ordenando por nombre del modelo
            IList<RentalSelectDTO> coches = await _context.Car
                .Include(c => c.Model)          // Se incluye la entidad Model para acceder al nombre del modelo
                .Where(c =>
                    (modelo == null || c.Model.Name.Contains(modelo)) &&            // Filtro por nombre si modelo está definido
                    (precio == null || c.RentingPrice <= precio)                    // Filtro por precio si precio está definido
                )
                .OrderBy(c => c.Model.Name)             // Se ordenan los resultados alfabéticamente por modelo
                .Select(c => new RentalSelectDTO(
                    c.Id,
                    c.Model.Name,
                    c.Manufacturer,
                    c.FuelType,
                    c.RentingPrice,
                    c.Color
                ))
                .ToListAsync();             // Ejecución de la consulta y transformación a lista

            // Devolvemos la lista resultante en formato JSON con código 200 OK
            return Ok(coches);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ReviewSelectDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaReview(string? fabricante, string? fuelType)
        {
            // ------------------------------------------------------------
            // PRIMERA VALIDACIÓN: COMPROBAR SI EXISTE LA TABLA DE CARS
            // Y QUE HAYA COCHES EN LA BASE DE DATOS.
            //
            // Esto evita que el programa intente buscar coches si la base
            // no está cargada o está vacía, lo cual causaría errores.
            // ------------------------------------------------------------
            if (_context.Car == null || !_context.Car.Any())
            {
                // Creamos un objeto de error estructurado para enviarlo al usuario
                var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "Error: Cars table does not exist or no cars available" } }
            });

                // Respondemos con BadRequest porque NO es un error de ID sino de estado
                return BadRequest(problemDetails);
            }




            // ------------------------------------------------------------
            // SI LLEGAMOS AQUÍ, SIGNIFICA QUE SÍ HAY COCHES EN LA BASE
            // AHORA LOS FILTRAMOS SEGÚN LOS PARÁMETROS RECIBIDOS
            //
            // fabricante → filtra por marca (Audi, Honda, Toyota…)
            // fuelType   → filtra por tipo de combustible (Gasolina, Híbrido…)
            //
            // Los parámetros pueden ser nulos, eso significa "no filtrar"
            // ------------------------------------------------------------
            IList<ReviewSelectDTO> coches = await _context.Car

                // Para cada coche, incluimos también datos del modelo del coche
                .Include(c => c.Model)

                // FILTROS: Sólo se aplica si el parámetro NO es null
                .Where(c =>
                    (fabricante == null || c.Manufacturer.Contains(fabricante)) &&
                    (fuelType == null || c.FuelType.Contains(fuelType))
                )

                // Ordenamos para devolver lista ordenada por modelo y marca
                .OrderBy(c => c.Model.Name)
                .ThenBy(c => c.Manufacturer)

                // Convertimos los datos de la base en un DTO
                // (Este objeto es la versión "visible" de la información)
                .Select(c => new ReviewSelectDTO(
                    c.Id,                  // ID del coche
                    c.Model.Name,          // Nombre del modelo (Ej: "Q5")
                    c.CarClass,            // Clase del coche (Turismo, SUV…)
                    c.Manufacturer,        // Marca (Audi, Toyota…)
                    c.FuelType,            // Tipo combustible
                    c.Color                // Color
                ))
                // Finalmente convertimos la consulta en una lista
                .ToListAsync();

            return Ok(coches);
        }


        




    }
}
