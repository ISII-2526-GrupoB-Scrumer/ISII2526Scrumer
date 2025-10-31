using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchaseController> _logger;


        public PurchaseController(ApplicationDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }
        // Paso 7 - GetPurchase (Detalle)
        // ===============================================================
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            if (_context.Purchase == null)
            {
                _logger.LogError("Error: Purchase table does not exist");
                return NotFound();
            }

            var purchase = await _context.Purchase
                .Where(p => p.Id == id)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Car)
                        .ThenInclude(c => c.Model)
                .Select(p => new PurchaseDetailDTO(
                    p.Id,
                    p.PurchasingDate,
                    p.PaymentMethod,
                    p.DriverType,
                    p.DeliveryCarDealer,
                    p.Country,
                    p.Client.UserName,
                    p.PurchasingPrice,
                    p.PurchaseItems.Select(pi => new PurchaseItemDTO(
                        pi.Car.Id,
                        pi.Car.Model.Name,
                        pi.Car.PurchasingPrice,
                        pi.Quantity
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }

            return Ok(purchase);
        }

        // ===============================================================
        // Paso 5 - CreatePurchase (Post)
        // ===============================================================
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreatePurchase(PurchaseCreateDTO purchaseCreate)
        {
            // 1️⃣ Validaciones
            if (purchaseCreate.PurchaseItems.Count == 0)
                ModelState.AddModelError("PurchaseItems", "Error: Debes incluir al menos un coche para comprar.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == purchaseCreate.ClientId);
            if (user == null)
                ModelState.AddModelError("ClientId", "Error: El usuario no existe.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // 2️⃣ Crear objeto Purchase
            var purchase = new Purchase
            {
                PurchasingDate = purchaseCreate.PurchasingDate == default ? DateTime.Now : purchaseCreate.PurchasingDate,
                PaymentMethod = purchaseCreate.PaymentMethod,
                DriverType = purchaseCreate.DriverType,
                DeliveryCarDealer = purchaseCreate.DeliveryCarDealer,
                Country = purchaseCreate.Country,
                Client = user,
                PurchaseItems = new List<PurchaseItem>()
            };

            // 3️⃣ Cargar coches
            var carIds = purchaseCreate.PurchaseItems.Select(pi => pi.CarId).ToList();

            var cars = await _context.Car
                .Where(c => carIds.Contains(c.Id))
                .Include(c => c.Model)
                .ToListAsync();

            foreach (var item in purchaseCreate.PurchaseItems)
            {
                var car = cars.FirstOrDefault(c => c.Id == item.CarId);
                if (car == null)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error: El coche con ID {item.CarId} no existe.");
                    continue;
                }

                purchase.PurchaseItems.Add(new PurchaseItem
                {
                    CarId = car.Id,
                    Car = car,
                    Quantity = item.Quantity,
                    Purchase = purchase
                });
            }

            // 4️⃣ Calcular precio total
            purchase.PurchasingPrice = purchase.PurchaseItems.Sum(pi => pi.Car.PurchasingPrice * pi.Quantity);

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // 5️⃣ Guardar en la base de datos
            _context.Add(purchase);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la compra en base de datos");
                return Conflict("Error al guardar la compra. " + ex.Message);
            }

            // 6️⃣ Construir DTO de detalle
            var detailDTO = new PurchaseDetailDTO(
                purchase.Id,
                purchase.PurchasingDate,
                purchase.PaymentMethod,
                purchase.DriverType,
                purchase.DeliveryCarDealer,
                purchase.Country,
                user.UserName,
                purchase.PurchasingPrice,
                purchaseCreate.PurchaseItems
            );

            return CreatedAtAction(nameof(GetPurchase), new { id = purchase.Id }, detailDTO);
        }
    }
}
