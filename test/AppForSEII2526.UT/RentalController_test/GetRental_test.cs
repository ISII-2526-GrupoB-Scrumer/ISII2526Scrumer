using AppForSEII2526.API;
using AppForSEII2526;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526
{
    public class GetRental_test : AppForSEII25264SqliteUT
    {
        public GetRental_test()
        {
            // ====== Datos base ======
            var models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
            };

            var cars = new List<Car>()
            {
                new Car("Turismo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 18000, 2, 4, 180, 18, models[0]),
                new Car("Turismo","Negro","","1.5","Diesel", 2,"Deportivo","Peugeot", 15000, 2, 4, 200, 15, models[1])
            };

            var user = new ApplicationUser("Albacete 1", "600000001", "Manuel", "Garcia", "manolito") { Id="U1"};

            // ====== Rental ======
            var rental = new Rental
            {
                Id = 1,
                Client = user,
                DeliveryCarDealer = "Albacete Center",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                RentingDate = DateTime.Now,
                PaymentMethod = "Tarjeta",
                TotalPrice = 360m,
                RentalItems = new List<RentalItem>()
            };

            rental.RentalItems.Add(new RentalItem
            {
                Car = cars[0],
                Quantity = 2
            });

            // ====== Guardar en DB ======
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(user);
            _context.Add(rental);
            _context.SaveChanges();
        }

        //RENTAL NO ENCONTRADO
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test()
        {
            var mock = new Mock<ILogger<RentalController>>();
            ILogger<RentalController> logger = mock.Object;

            var controller = new RentalController(_context, logger);

            var result = await controller.GetRental(999);

            Assert.IsType<NotFoundResult>(result);
        }

        //RENTAL ENCONTRADO
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_Found_test()
        {
            var mock = new Mock<ILogger<RentalController>>();
            ILogger<RentalController> logger = mock.Object;
            var controller = new RentalController(_context, logger);

            // ====== Expected DTO ======
            var expectedRental = new RentalDetailDTO(
                1,
                "U1",
                "Manuel",
                "Garcia",
                "Albacete 1",
                "Tarjeta",
                DateTime.Today,                 // RentingDate (aproximado)
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(3),
                360m,
                "Albacete Center",
                new List<RentalItemDTO>
                {
                    new RentalItemDTO(1, "Q5", 180m, 2, "Audi")
                }
            );

            // ====== Act ======
            var result = await controller.GetRental(1);

            // ====== Assert ======
            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentalDTOActual = Assert.IsType<RentalDetailDTO>(okResult.Value);

            Assert.Equal(expectedRental.Id, rentalDTOActual.Id);
            Assert.Equal(expectedRental.DeliveryCarDealer, rentalDTOActual.DeliveryCarDealer);
            Assert.Equal(expectedRental.TotalPrice, rentalDTOActual.TotalPrice);

            // Verificar también que los RentalItems coinciden
            Assert.Single(rentalDTOActual.RentalItems);
            var expectedItem = expectedRental.RentalItems.First();
            var actualItem = rentalDTOActual.RentalItems.First();

            Assert.Equal(expectedItem.CarId, actualItem.CarId);
            Assert.Equal(expectedItem.CarModel, actualItem.CarModel);
            Assert.Equal(expectedItem.Manufacturer, actualItem.Manufacturer);
            Assert.Equal(expectedItem.Quantity, actualItem.Quantity);
        }
    }
}
