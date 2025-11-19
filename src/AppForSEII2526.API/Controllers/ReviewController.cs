using AppForSEII2526.API.DTOs.ReviewDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    // Esta línea indica que este archivo responde a peticiones web tipo API
    // y que la ruta base será "/api/Review"
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        // ESTAS VARIABLES GUARDAN LA CONEXIÓN A LA BASE
        // Y EL SISTEMA DE LOGS PARA MOSTRAR ERRORES
        //--------------------------------------------
        private readonly ApplicationDbContext _context;          // Aquí guardamos acceso a la BD   
        private readonly ILogger<ReviewController> _logger;      // Esto sirve para imprimir errores en consola



        // ESTE ES EL CONSTRUCTOR: SE EJECUTA AL CREAR EL CONTROLADOR.
        // AQUÍ RECIBIMOS LA BASE DE DATOS Y EL LOGGER PARA PODER USARLOS
        // -------------------------------------------------------------
        public ReviewController(ApplicationDbContext context, ILogger<ReviewController> logger)
        {
            _context = context;  // Guardamos la BD en una variable de clase
            _logger = logger;    // Guardamos el logger
        }



        // =============================================================
        //  ************************* GET REVIEW ************************
        //  PASO 7 DEL CASO DE USO → MOSTRAR LA RESEÑA
        //  ESTE MÉTODO RECIBE UN ID Y DEVUELVE LOS DATOS DE LA RESEÑA
        // =============================================================

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReview(int id)
        {

            // Comprobamos si la tabla existe en la base de datos.
            // Si por algún motivo está vacía o no cargó, devolvemos error.
            if (_context.Review == null)
            {
                _logger.LogError("Error: Review table does not exist");
                return NotFound(); // No hay reseñas
            }


            // --------------------------------------------------
            // AQUÍ BUSCAMOS LA RESEÑA CON EL ID QUE NOS HAN PASADO
            // --------------------------------------------------
            var review = await _context.Review

                // Filtramos para que solo coja la reseña con ese id
                .Where(r => r.Id == id)

                // Traemos también los coches asociados a esa reseña
                .Include(r => r.Cars)

                    // Por cada coche, traemos la información completa del coche
                    .ThenInclude(ri => ri.Car)

                        // Y además accedemos al modelo del coche
                        .ThenInclude(c => c.Model)


                // Aquí convertimos los datos de la base a un DTO (formato para mostrar al usuario)    
                .Select(r => new ReviewDetailDTO(
                    r.Id,                   // ID de la reseña
                    r.Client.Id,            // Usuario que la hizo
                    r.Created,              // Fecha en que se hizo
                    r.Client.Name,          // Nombre del cliente
                    r.Country,
                    r.DriverType,

                    // Lista de coches reseñados con toda la info visible
                    r.Cars.Select(ri => new ReviewItemDTO(
                        ri.Car.Id,
                        ri.Car.Model.Name,
                        ri.Car.Manufacturer,
                        ri.Car.FuelType,
                        ri.Car.Color,
                        ri.Description,     // Lo que escribió el usuario
                        ri.Rating           // Puntuación
                    )).ToList()

                ))

                // Coge solo el primero (si no existe dará null)
                .FirstOrDefaultAsync();



            // Si la reseña no existe, avisamos
            if (review == null)
            {
                _logger.LogError($"Error: Review with id {id} does not exist");
                return NotFound();
            }


            // Si existe, la enviamos al usuario
            return Ok(review);
        }






        // =============================================================
        //  *********************** CREAR REVIEW ************************
        //  PASO 5 DEL CASO DE USO → GUARDAR UNA NUEVA RESEÑA
        // =============================================================

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReview(ReviewCreateDTO reviewCreate)
        {
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == reviewCreate.ClientId);

            if (user == null)
                ModelState.AddModelError("Client", "El usuario indicado no existe.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));


            if (reviewCreate.ReviewItems.Count == 0)
                ModelState.AddModelError("ReviewItems", "Error: Debes incluir al menos un coche en la review.");

            if (string.IsNullOrWhiteSpace(reviewCreate.Name))
                ModelState.AddModelError("Name", "Error: Debes indicar tu nombre.");

            if (string.IsNullOrWhiteSpace(reviewCreate.Country))
                ModelState.AddModelError("Country", "Error: Debes indicar tu país.");

            if (string.IsNullOrWhiteSpace(reviewCreate.DriverType))
                ModelState.AddModelError("DriverType", "Error: Debes indicar el tipo de conductor.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var review = new Review
            {
                Created = DateTime.Now,
                Country = reviewCreate.Country,
                DriverType = reviewCreate.DriverType,
                Client = user,
                Cars = new List<ReviewItem>()
            };


            var carIds = reviewCreate.ReviewItems.Select(ri => ri.CarId).ToList();

            var cars = await _context.Car
                .Where(c => carIds.Contains(c.Id))
                .Include(c => c.Model)
                .ToListAsync();

            foreach (var item in reviewCreate.ReviewItems)
            {
                var car = cars.FirstOrDefault(c => c.Id == item.CarId);
                if (car == null)
                {
                    ModelState.AddModelError("ReviewItems", $"Error: El coche con ID {item.CarId} no existe.");
                    continue;
                }

                review.Cars.Add(new ReviewItem
                {
                    CarId = car.Id,
                    Car = car,
                    Description = item.Description,
                    Rating = item.Rating,
                    Review = review
                });
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Add(review);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la review en base de datos");
                return Conflict("Error: No se pudo guardar la review. " + ex.Message);
            }

            var detailDTO = new ReviewDetailDTO(
                review.Id,
                review.Client.Id,
                review.Created,
                reviewCreate.Name,
                review.Country,
                review.DriverType,
                reviewCreate.ReviewItems
            );

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, detailDTO);
        }

    }
}
