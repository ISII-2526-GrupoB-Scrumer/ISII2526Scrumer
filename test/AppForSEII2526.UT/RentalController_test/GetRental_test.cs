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
            // Se crean modelos de coches para insertarlos en la BD en memoria
            var models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
            };

            // Se crean coches asociados a los modelos anteriores
            var cars = new List<Car>()
            {
                new Car("Turismo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 18000, 2, 4, 180, 18, models[0]),
                new Car("Turismo","Negro","","1.5","Diesel", 2,"Deportivo","Peugeot", 15000, 2, 4, 200, 15, models[1])
            };

            // Se crea un usuario cliente
            var user = new ApplicationUser("Albacete 1", "600000001", "Manuel", "Garcia", "manolito") { Id="U1"};

            // ====== Rental ======
            // Se construye un alquiler asociado al usuario anterior
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

            // Se agrega un coche alquilado dentro del rental
            rental.RentalItems.Add(new RentalItem
            {
                Car = cars[0],
                Quantity = 2
            });

            // ====== Guardar en DB ======
            // Guardamos todo en la BD en memoria para que los tests puedan consultarlo
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(user);
            _context.Add(rental);
            _context.SaveChanges();
        }

        //RENTAL NO ENCONTRADO
        // Verifica que si el ID no existe, el controlador devuelve NotFound()
        [Theory]
        [InlineData(999)]
        [InlineData(-1)]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test(int id)
        {
            var mock = new Mock<ILogger<RentalController>>();
            ILogger<RentalController> logger = mock.Object;

            // Se instancia el controlador con la BD en memoria
            var controller = new RentalController(_context, logger);

            // Se pide un rental inexistente
            var result = await controller.GetRental(id);

            // Se comprueba que la respuesta es 404
            Assert.IsType<NotFoundResult>(result);
        }
        
        //RENTAL ENCONTRADO
        // Comprueba que el alquiler existe y que el DTO devuelto coincide con lo esperado
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_Found_test()
        {
            var mock = new Mock<ILogger<RentalController>>();
            ILogger<RentalController> logger = mock.Object;
            var controller = new RentalController(_context, logger);

            // ====== Expected DTO ======
            // Se construye manualmente el DTO esperado
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
            // Se ejecuta la llamada al controlador
            var result = await controller.GetRental(1);

            // ====== Assert ======
            // Se asegura que la respuesta es 200 OK
            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentalDTOActual = Assert.IsType<RentalDetailDTO>(okResult.Value);

            // Se compara el objeto devuelto con el esperado usando Equals()
            Assert.Equal(expectedRental, rentalDTOActual);

            // También comparamos el elemento dentro de la lista
            // Verificar también que los RentalItems coinciden
            var expectedItem = expectedRental.RentalItems.First();
            var actualItem = rentalDTOActual.RentalItems.First();
            Assert.Equal(expectedItem, actualItem);
        }
    }
}
