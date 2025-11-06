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
            // --- MODELOS ---
            var Models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
                new Model("3", "Civic"),
                new Model("4", "Corolla")
            };

            // --- COCHES ---
            var Cars = new List<Car>()
            {
                new Car("Turismo","Blanco","","1.8","Gasolina",1,"Estandar","Audi",18000,2,4,180,18,Models[0]),
                new Car("Turismo","Negro","","1.5","Diesel",2,"Deportivo","Peugeot",15000,2,4,200,15,Models[1]),
                new Car("Familiar","Azul","","2.0","Hibrido",3,"Estandar","Honda",22000,4,5,160,20,Models[2]),
                new Car("Familiar","Rojo","","1.6","Gasolina",4,"Estandar","Toyota",17000,4,5,140,17,Models[3])
            };

            // --- USUARIOS ---
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser("Albacete 1","600000001","Manuel","Garcia","manolito"),
                new ApplicationUser("Albacete 2","600000002","Lucia","Martinez","luci")
            };

            // --- REVIEW ---
            var review = new Review(1, new DateTime(2025, 10, 20))
            {
                Country = "España",
                DriverType = "Habitual",
                Client = users[0]
            };

            // --- REVIEW ITEMS ---
            var reviewItems = new List<ReviewItem>()
            {
                new ReviewItem(Cars[0].Id, "Muy buen coche, cómodo", 5, review.Id),
                new ReviewItem(Cars[2].Id, "Consumo eficiente", 4, review.Id)
            };

            review.Cars = reviewItems;

            // --- Guardamos en la base de datos in-memory ---
            _context.AddRange(Models);
            _context.AddRange(Cars);
            _context.AddRange(users);
            _context.Add(review);
            _context.AddRange(reviewItems);
            _context.SaveChanges();
        }

        // --- TEST CASES ---
        public static IEnumerable<object[]> TestCasesFor_GetCochesParaReview_OK()
        {
            var cocheDTOs = new List<ReviewSelectDTO>()
            {
                new ReviewSelectDTO(1, "Q5", "Turismo", "Audi", "Gasolina", "Blanco"),
                new ReviewSelectDTO(2, "508", "Turismo", "Peugeot", "Diesel", "Negro"),
                new ReviewSelectDTO(3, "Civic", "Familiar", "Honda", "Hibrido", "Azul"),
                new ReviewSelectDTO(4, "Corolla", "Familiar", "Toyota", "Gasolina", "Rojo"),
            };


            var expected1 = cocheDTOs.OrderBy(c => c.ModelName).ToList();
            var expected2 = cocheDTOs.Where(c => c.Manufacturer.Contains("Honda")).OrderBy(c => c.ModelName).ToList();
            var expected3 = cocheDTOs.Where(c => c.FuelType.Contains("Gasolina")).OrderBy(c => c.ModelName).ToList();
            var expected4 = cocheDTOs.Where(c => c.Manufacturer.Contains("Audi") && c.FuelType.Contains("Gasolina"))
                                     .OrderBy(c => c.ModelName)
                                     .ToList();

            return new List<object[]>
            {
                new object[] { null, null, expected1 },
                new object[] { "Honda", null, expected2 },
                new object[] { null, "Gasolina", expected3 },
                new object[] { "Audi", "Gasolina", expected4 },
            };
        }

        // --- TEST OK ---
        [Theory]
        [MemberData(nameof(TestCasesFor_GetCochesParaReview_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaReview_OK_test(string? fabricante, string? fuelType, IList<ReviewSelectDTO> expected)
        {
            var controller = new CarsController(_context, null);

            var result = await controller.GetCochesParaReview(fabricante, fuelType);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ReviewSelectDTO>>(okResult.Value);

            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ModelName, actual[i].ModelName);
                Assert.Equal(expected[i].Manufacturer, actual[i].Manufacturer);
                Assert.Equal(expected[i].FuelType, actual[i].FuelType);
                Assert.Equal(expected[i].CarClass, actual[i].CarClass);
                Assert.Equal(expected[i].Color, actual[i].Color);
            }
        }

        // --- TEST BAD REQUEST ---
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaReview_BadRequest_test()
        {
            var mockLogger = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mockLogger.Object;
            var controller = new CarsController(_context, logger);

            _context.Car.RemoveRange(_context.Car);
            await _context.SaveChangesAsync();

            var result = await controller.GetCochesParaReview("Audi", "Gasolina");

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorMessage = problemDetails.Errors.First().Value[0];

            Assert.Equal("Error: Cars table does not exist or no cars available", errorMessage);
        }
    }
}
