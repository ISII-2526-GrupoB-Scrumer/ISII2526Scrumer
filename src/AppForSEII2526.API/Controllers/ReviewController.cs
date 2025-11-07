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


        //PASO 7
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
                    r.Country,
                    r.DriverType,
                    r.Client.UserName, 
                    r.Cars.Select(ri => new ReviewItemDTO(
                        ri.Car.Id,
                        ri.Car.Model.Name,
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
          
            if (reviewCreate.ReviewItems.Count == 0)
                ModelState.AddModelError("ReviewItems", "Error: Debes incluir al menos un coche en la review.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == reviewCreate.ClientId);
            if (user == null)
                ModelState.AddModelError("ClientId", "Error: El usuario no existe.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var review = new Review
            {
                Created = reviewCreate.Created == default ? DateTime.Now : reviewCreate.Created,
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
                review.Country,
                review.DriverType,
                review.Client.UserName,
                reviewCreate.ReviewItems
            );

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, detailDTO);
        }

    }
}
