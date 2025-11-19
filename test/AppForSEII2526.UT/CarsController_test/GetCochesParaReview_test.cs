using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526
{
    public class GetCochesParaReview_test : AppForSEII25264SqliteUT
    {
        public GetCochesParaReview_test()
        {
            // --------------------------------------------------------------
            // SE CREAN LOS MODELOS DE COCHE QUE EXISTIRÁN EN LA BASE DE DATOS
            // Cada modelo representa el "nombre comercial" del coche
            // --------------------------------------------------------------
            var Models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
                new Model("3", "Civic"),
                new Model("4", "Corolla")
            };

            // --------------------------------------------------------------
            // SE CREAN LOS COCHES (INSTANCIAS FÍSICAS DISPONIBLES)
            // --------------------------------------------------------------
            var Cars = new List<Car>()
            {
                new Car("Turismo","Blanco","","1.8","Gasolina",1,"Estandar","Audi",18000,2,4,180,18,Models[0]),
                new Car("Turismo","Negro","","1.5","Diesel",2,"Deportivo","Peugeot",15000,2,4,200,15,Models[1]),
                new Car("Familiar","Azul","","2.0","Hibrido",3,"Estandar","Honda",22000,4,5,160,20,Models[2]),
                new Car("Familiar","Rojo","","1.6","Gasolina",4,"Estandar","Toyota",17000,4,5,140,17,Models[3])
            };

            // --------------------------------------------------------------
            // SE CREAN USUARIOS. NO SE USAN DIRECTAMENTE PARA FILTRAR COCHES,
            // PERO ES NECESARIO PARA PODER CREAR UNA REVIEW REAL.
            // --------------------------------------------------------------
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser("Albacete 1","600000001","Manuel","Garcia","manolito"),
                new ApplicationUser("Albacete 2","600000002","Lucia","Martinez","luci")
            };

            // --------------------------------------------------------------
            // SE CREA UNA REVIEW EXISTENTE EN LA BASE DE DATOS
            // Esto sirve para simular que ya hay reseñas hechas
            // --------------------------------------------------------------
            var review = new Review(1, new DateTime(2025, 10, 20))
            {
                Country = "España",
                DriverType = "Habitual",
                Client = users[0]
            };

            // --------------------------------------------------------------
            // SE AÑADEN LOS COCHES QUE PARTICIPAN EN ESA REVIEW
            // (Esto NO afecta a los filtros del test, pero sí al estado del contexto)
            // --------------------------------------------------------------
            var reviewItems = new List<ReviewItem>()
            {
                new ReviewItem(Cars[0].Id, "Muy buen coche, cómodo", 5, review.Id),
                new ReviewItem(Cars[2].Id, "Consumo eficiente", 4, review.Id)
            };

            review.Cars = reviewItems;

            // --------------------------------------------------------------
            // GUARDAMOS TODO EN LA BASE DE DATOS EN MEMORIA (SQlite)
            // Esto permite que el test funcione como si hubiera una base real
            // --------------------------------------------------------------
            _context.AddRange(Models);
            _context.AddRange(Cars);
            _context.AddRange(users);
            _context.Add(review);
            _context.AddRange(reviewItems);
            _context.SaveChanges();
        }

       // =====================================================================
        //  * DATOS DE ENTRADA PARA LOS TEST DE ÉXITO (TEORÍA)
        // =====================================================================
        public static IEnumerable<object[]> TestCasesFor_GetCochesParaReview_OK()
        {
            // --------------------------------------------------------------
            // Esta lista representa lo que ESPERAMOS que devuelva el método
            // antes de aplicar filtros
            // --------------------------------------------------------------
            var cocheDTOs = new List<ReviewSelectDTO>()
            {
                new ReviewSelectDTO(1, "Q5", "Turismo", "Audi", "Gasolina", "Blanco"),
                new ReviewSelectDTO(2, "508", "Turismo", "Peugeot", "Diesel", "Negro"),
                new ReviewSelectDTO(3, "Civic", "Familiar", "Honda", "Hibrido", "Azul"),
                new ReviewSelectDTO(4, "Corolla", "Familiar", "Toyota", "Gasolina", "Rojo"),
            };

            // --------------------------------------------------------------
            // SE DEFINEN DIFERENTES ESCENARIOS DE FILTRADO
            // Cada uno simula parámetros distintos pasados a la API
            // --------------------------------------------------------------

            // Sin filtros → devuelve todos
            var expected1 = cocheDTOs.OrderBy(c => c.ModelName).ToList();

            // Filtrar solo por Honda
            var expected2 = cocheDTOs.Where(c => c.Manufacturer.Contains("Honda"))
                                     .OrderBy(c => c.ModelName)
                                     .ToList();

            // Filtrar solo por Gasolina
            var expected3 = cocheDTOs.Where(c => c.FuelType.Contains("Gasolina"))
                                     .OrderBy(c => c.ModelName)
                                     .ToList();

            // Filtro doble: Audi + Gasolina
            var expected4 = cocheDTOs.Where(c => c.Manufacturer.Contains("Audi") && c.FuelType.Contains("Gasolina"))
                                     .OrderBy(c => c.ModelName)
                                     .ToList();

            // --------------------------------------------------------------
            // DEVOLVEMOS LOS ESCENARIOS PARA QUE THEORY LOS EJECUTE AUTOMÁTICAMENTE
            // --------------------------------------------------------------
            return new List<object[]>
            {
                new object[] { null, null, expected1 },
                new object[] { "Honda", null, expected2 },
                new object[] { null, "Gasolina", expected3 },
                new object[] { "Audi", "Gasolina", expected4 },
            };
        }
        
        // =====================================================================
        // *** TEST PRINCIPAL: EL MÉTODO DEVUELVE LISTA FILTRADA CORRECTAMENTE
        // =====================================================================
        [Theory]
        [MemberData(nameof(TestCasesFor_GetCochesParaReview_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaReview_OK_test(string? fabricante, string? fuelType, IList<ReviewSelectDTO> expected)
        {
            // Se crea el controlador usando la BD configurada en el constructor
            var controller = new CarsController(_context, null);

            // Llamamos al método real que queremos probar
            var result = await controller.GetCochesParaReview(fabricante, fuelType);

            // El resultado debe ser HTTP 200 OK
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Obtenemos el contenido devuelto (la lista de coches)
            var actual = Assert.IsType<List<ReviewSelectDTO>>(okResult.Value);

            // Comparamos coche por coche
            Assert.Equal(expected, actual);
        }

        // =====================================================================
        // *** TEST ERROR: NO HAY COCHES EN LA BASE DE DATOS
        // =====================================================================
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaReview_BadRequest_test()
        {
            // Creamos logger para registrar error
            var mockLogger = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mockLogger.Object;

            var controller = new CarsController(_context, logger);

            // Eliminamos todos los coches para simular base vacía
            _context.Car.RemoveRange(_context.Car);
            await _context.SaveChangesAsync();

            // Llamamos al método con filtros (que ahora no pueden aplicarse)
            var result = await controller.GetCochesParaReview("Audi", "Gasolina");

            // Debe devolver BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Leemos el mensaje de error
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorMessage = problemDetails.Errors.First().Value[0];

            // Comprobamos que el mensaje coincide con el esperado
            Assert.Equal("Error: Cars table does not exist or no cars available", errorMessage);
        }
    }
}
