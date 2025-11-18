using AppForSEII2526.API.DTOs.ReviewDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewController> _logger;


        public ReviewController(ApplicationDbContext context, ILogger<ReviewController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // Paso 7

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReview(int id)
        {
            if (_context.Review == null)
            {
                _logger.LogError("Error: Review table does not exist");
                return NotFound();
            }

            var review = await _context.Review
                .Where(r => r.Id == id)
                .Include(r => r.Cars)
                    .ThenInclude(ri => ri.Car)
                        .ThenInclude(c => c.Model)
                .Select(r => new ReviewDetailDTO(
                    r.Id,
                    r.Created,
                    r.Client.Name,
                    r.Country,
                    r.DriverType,
                    r.Cars.Select(ri => new ReviewItemDTO(
                        ri.Car.Id,
                        ri.Car.Model.Name,
                        ri.Car.Manufacturer,
                        ri.Car.FuelType,
                        ri.Car.Color,
                        ri.Description,
                        ri.Rating
                    )).ToList()

                ))
                .FirstOrDefaultAsync();

            if (review == null)
            {
                _logger.LogError($"Error: Review with id {id} does not exist");
                return NotFound();
            }

            return Ok(review);
        }


        // Paso 5: 

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
