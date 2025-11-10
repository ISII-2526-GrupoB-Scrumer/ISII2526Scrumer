using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API;
using Humanizer;

namespace AppForSEII2526
{
    public class GetCochesParaAlquilar_test : AppForSEII25264SqliteUT
    {
        public GetCochesParaAlquilar_test()
        {

            //MODELOS
            var Models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
                new Model("3", "Civic"),
                new Model("4", "Corolla")
            };


            //Coches
            var Cars = new List<Car>()
            {
                new Car("Turimo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 18000, 2, 4, 180, 18, Models[0]),
                new Car("Turimo", "Negro", "", "1.5", "Diesel", 2, "Deportivo", "Peugeot", 15000, 2, 4, 200, 15, Models[1]),
                new Car("Familiar", "Azul", "", "2.0", "Hibrido", 3, "Estandar", "Honda", 22000, 4, 5, 160, 20, Models[2]),
                new Car("Familiar", "Rojo", "", "1.6", "Gasolina", 4, "Estandar", "Toyota", 17000, 4, 5, 140, 17, Models[3])
            };

            //Usuarios
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser("Albacete 1","600000001","Manuel","Garcia","manolito"),
                new ApplicationUser("Albacete 2","600000002","Lucia","Martinez","luci"),
                new ApplicationUser("Albacete 3","600000003","Carlos","Lopez","carlitos"),
                new ApplicationUser("Albacete 4","600000004","Ana","Gonzalez","anita")
            };

            var rental = new Rental(
                "Albacete center",    //DeliveryCarDealer
                new DateTime(2025, 10, 20), //EndDate
                1,  //id
                new DateTime(2025, 10, 15), //RentingDate
                new DateTime(2025, 10, 16), //StartDate
                180, //TotalPrice
                "Tarjeta"); //PaymentMethod

            rental.Client = users[0];


            var rentalItems = new List<RentalItem>()
            {
                new RentalItem(Cars[0].Id,1,rental.Id),
                new RentalItem(Cars[1].Id,1,rental.Id)
            };

            rental.RentalItems = rentalItems;



            _context.AddRange(Models);
            _context.AddRange(Cars);
            _context.AddRange(users);
            _context.Add(rental);
            _context.AddRange(rentalItems);
            _context.SaveChanges();

        }


        

        public static IEnumerable<object[]> TestCasesFor_GetCochesParaAlquilar_OK()
        {
            var cocheDTOs = new List<RentalSelectDTO>()
            {
                new RentalSelectDTO(1, "Q5", "Audi", "Gasolina", 180m, "Blanco"),
                new RentalSelectDTO(2, "508", "Peugeot", "Diesel", 200m, "Negro"),
                new RentalSelectDTO(3, "Civic", "Honda", "Hibrido", 160m, "Azul"),
                new RentalSelectDTO(4, "Corolla", "Toyota", "Gasolina", 140m, "Rojo"),
            };

            var expected1 = cocheDTOs
                .OrderBy(c => c.ModelName)
                .ToList();

            var expected2 = cocheDTOs
                .Where(c => c.ModelName.Contains("Civic"))
                .OrderBy(c => c.ModelName)
                .ToList();

            var expected3 = cocheDTOs
                .Where(c => c.RentingPrice <= 150m)
                .OrderBy(c => c.ModelName)
                .ToList();

            var expected4 = cocheDTOs
               .Where(c => c.ModelName.Contains("Q5") && c.RentingPrice <= 180m)
               .OrderBy(c => c.ModelName)
               .ToList();


            var allTests = new List<object[]>
            {
                new object[] { null, null, expected1 },      // sin filtros
                new object[] { "Civic", null, expected2 },   // filtro por modelo
                new object[] { null, 150m, expected3 },      // filtro por precio
                new object[] { "Q5", 180m, expected4 },      // filtro combinado
            };

            return allTests;
        }


        [Theory]
        [MemberData(nameof(TestCasesFor_GetCochesParaAlquilar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaAlquilar_OK_test(string? modelo, decimal? precio, IList<RentalSelectDTO> expected)
        {
            // Arrange
            var controller = new CarsController(_context, null);

            // Act
            var result = await controller.GetCochesParaAlquilar(modelo, precio);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<RentalSelectDTO>>(okResult.Value);

            // Comprobamos que las listas sean iguales (mismo número y orden)
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ModelName, actual[i].ModelName);
                Assert.Equal(expected[i].Manufacturer, actual[i].Manufacturer);
                Assert.Equal(expected[i].FuelType, actual[i].FuelType);
                Assert.Equal(expected[i].RentingPrice, actual[i].RentingPrice);
            }
        }


        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaAlquilar_BadRequest_test()
        {
            // Arrange
            // Simulamos un logger para el controlador
            var mockLogger = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mockLogger.Object;

            // Creamos el controlador con logger (por si lanza mensajes de error)
            var controller = new CarsController(_context, logger);

            //Para provocar el error, eliminamos los coches del contexto
            _context.Car.RemoveRange(_context.Car);
            await _context.SaveChangesAsync();

            // Act
            var result = await controller.GetCochesParaAlquilar("Civic", null);

            // Assert
            // Verificamos que devuelve un BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            // Comprobamos el mensaje de error
            var errorMessage = problemDetails.Errors.First().Value[0];
            Assert.Equal("Error: Cars table does not exist or no cars available", errorMessage);
        }




    }
}
