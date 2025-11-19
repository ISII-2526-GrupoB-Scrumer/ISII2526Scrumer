using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API;
using Humanizer;

namespace AppForSEII2526
{
    // Clase de pruebas unitarias para el método GetCochesParaAlquilar del CarsController
    public class GetCochesParaAlquilar_test : AppForSEII25264SqliteUT
    {
        // Constructor que inicializa una base de datos en memoria con datos de prueba
        public GetCochesParaAlquilar_test()
        {

            //MODELOS
            // Se crean modelos de coche con distintos nombres para usar en los vehículos
            var Models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
                new Model("3", "Civic"),
                new Model("4", "Corolla")
            };


            //Coches
            // Se crean varios coches asociados a los modelos anteriores
            var Cars = new List<Car>()
            {
                new Car("Turimo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 18000, 2, 4, 180, 18, Models[0]),
                new Car("Turimo", "Negro", "", "1.5", "Diesel", 2, "Deportivo", "Peugeot", 15000, 2, 4, 200, 15, Models[1]),
                new Car("Familiar", "Azul", "", "2.0", "Hibrido", 3, "Estandar", "Honda", 22000, 4, 5, 160, 20, Models[2]),
                new Car("Familiar", "Rojo", "", "1.6", "Gasolina", 4, "Estandar", "Toyota", 17000, 4, 5, 140, 17, Models[3])
            };

            //Usuarios
            // Se crean usuarios de ejemplo para simular clientes
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser("Albacete 1","600000001","Manuel","Garcia","manolito"),
                new ApplicationUser("Albacete 2","600000002","Lucia","Martinez","luci"),
                new ApplicationUser("Albacete 3","600000003","Carlos","Lopez","carlitos"),
                new ApplicationUser("Albacete 4","600000004","Ana","Gonzalez","anita")
            };

            //Rental
            // Se crea un alquiler ya existente en la BD para comprobar disponibilidad más adelante
            var rental = new Rental(
                "Albacete center",    //DeliveryCarDealer
                new DateTime(2025, 10, 20), //EndDate
                1,  //id
                new DateTime(2025, 10, 15), //RentingDate
                new DateTime(2025, 10, 16), //StartDate
                180, //TotalPrice
                "Tarjeta"); //PaymentMethod

            rental.Client = users[0];

            //Items asociados al alquiler
            var rentalItems = new List<RentalItem>()
            {
                new RentalItem(Cars[0].Id,1,rental.Id),
                new RentalItem(Cars[1].Id,1,rental.Id)
            };

            rental.RentalItems = rentalItems;


            // Se guardan los datos en la BD en memoria
            _context.AddRange(Models);
            _context.AddRange(Cars);
            _context.AddRange(users);
            _context.Add(rental);
            _context.AddRange(rentalItems);
            _context.SaveChanges();

        }


        //GENERADOR DE CASOS DE PRUEBA
        // Devuelve varios escenarios con diferentes filtros para testear el GET
        public static IEnumerable<object[]> TestCasesFor_GetCochesParaAlquilar_OK()
        {
            // Lista esperada de coches convertidos a DTO
            var cocheDTOs = new List<RentalSelectDTO>()
            {
                new RentalSelectDTO(1, "Q5", "Audi", "Gasolina", 180m, "Blanco"),
                new RentalSelectDTO(2, "508", "Peugeot", "Diesel", 200m, "Negro"),
                new RentalSelectDTO(3, "Civic", "Honda", "Hibrido", 160m, "Azul"),
                new RentalSelectDTO(4, "Corolla", "Toyota", "Gasolina", 140m, "Rojo"),
            };

            // Sin filtros -> devuelve todos
            var expected1 = cocheDTOs
                .OrderBy(c => c.ModelName)
                .ToList();

            // Filtro por modelo
            var expected2 = cocheDTOs
                .Where(c => c.ModelName.Contains("Civic"))
                .OrderBy(c => c.ModelName)
                .ToList();

            // Filtro por precio
            var expected3 = cocheDTOs
                .Where(c => c.RentingPrice <= 150m)
                .OrderBy(c => c.ModelName)
                .ToList();

            // Filtro combinado
            var expected4 = cocheDTOs
               .Where(c => c.ModelName.Contains("Q5") && c.RentingPrice <= 180m)
               .OrderBy(c => c.ModelName)
               .ToList();

            // Se devuelven como colección de tests parametrizados
            var allTests = new List<object[]>
            {
                new object[] { null, null, expected1 },      // sin filtros
                new object[] { "Civic", null, expected2 },   // filtro por modelo
                new object[] { null, 150m, expected3 },      // filtro por precio
                new object[] { "Q5", 180m, expected4 },      // filtro combinado
            };

            return allTests;
        }

        // ===== TEST PRINCIPAL (OK) =====
        // Comprueba que el método devuelve la lista correcta de coches según filtros
        [Theory]
        [MemberData(nameof(TestCasesFor_GetCochesParaAlquilar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaAlquilar_OK_test(string? modelo, decimal? precio, IList<RentalSelectDTO> expected)
        {
            // Arrange -> Preparamos el escenario
            var controller = new CarsController(_context, null);

            // Act -> Ejecutamos la accion a testear
            var result = await controller.GetCochesParaAlquilar(modelo, precio);

            // Assert -> Comprobamos el resultado esperado
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<RentalSelectDTO>>(okResult.Value);

            // Comprobamos que las listas sean iguales (mismo número y orden)
            Assert.Equal(expected,actual);
            
        }


        // ===== TEST DE ERROR =====
        // Si no hay coches en la BD, debe devolver BadRequest con mensaje
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaAlquilar_BadRequest_test()
        {
            // Arrange -> Preparamos el escenario
            // Simulamos un logger para el controlador
            var mockLogger = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mockLogger.Object;

            // Creamos el controlador con logger (por si lanza mensajes de error)
            var controller = new CarsController(_context, logger);

            //Para provocar el error, eliminamos los coches del contexto
            _context.Car.RemoveRange(_context.Car);
            await _context.SaveChangesAsync();

            // Act -> Ejecutamos la accion a testear
            var result = await controller.GetCochesParaAlquilar("Civic", null);

            // Assert -> Comprobamos el resultado esperado
            // Verificamos que devuelve un BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            // Comprobamos el mensaje de error
            var errorMessage = problemDetails.Errors.First().Value[0];
            Assert.Equal("Error: Cars table does not exist or no cars available", errorMessage);
        }




    }
}
