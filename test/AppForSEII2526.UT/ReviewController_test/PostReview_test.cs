using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526
{
    public class PostReview_test : AppForSEII25264SqliteUT
    {
        public PostReview_test()
        {
            var models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "Civic"),
            };

            var cars = new List<Car>()
            {
                new Car("Turismo","Blanco","","1.8","Gasolina",1,"Estandar","Audi",18000,2,4,180,18,models[0]),
                new Car("Familiar","Azul","","2.0","Hibrido",2,"Estandar","Honda",22000,4,5,160,20,models[1])
            };

            var user = new ApplicationUser("Calle Mayor 1", "600000001", "Manuel", "Garcia", "manolito");

            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(user);
            _context.SaveChanges();
        }

        // Test Correcto
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateReview_OK_test()
        {
            var logger = new Mock<ILogger<ReviewController>>().Object;
            var controller = new ReviewController(_context, logger);

            var reviewCreate = new ReviewCreateDTO
            {
                ClientId = "manolito",
                Created = new DateTime(2025, 10, 15),
                Name = "Manuel",
                Country = "España",
                DriverType = "Habitual",
                ReviewItems = new List<ReviewItemDTO>
                {
                    new ReviewItemDTO(1, "Q5", "Audi", "Gasolina", "Blanco", "Buen coche", 5),
                    new ReviewItemDTO(2, "Civic", "Honda", "Hibrido", "Azul", "Cómodo y ágil", 4)
                }
            };

            var result = await controller.CreateReview(reviewCreate);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<ReviewDetailDTO>(createdResult.Value);

            Assert.Equal(reviewCreate.Country, dto.Country);
            Assert.Equal(reviewCreate.DriverType, dto.DriverType);
            Assert.Equal("Manuel", dto.Name);
            Assert.Equal(2, dto.ReviewItems.Count);
        }

        // Test Fallo
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateReview_BadRequest_test()
        {
            var logger = new Mock<ILogger<ReviewController>>().Object;
            var controller = new ReviewController(_context, logger);

            var reviewCreate = new ReviewCreateDTO
            {
                ClientId = "manolito",
                Created = DateTime.Now,
                Name = "Manuel",
                Country = "España",
                DriverType = "Habitual",
                ReviewItems = new List<ReviewItemDTO>()
            };

            var result = await controller.CreateReview(reviewCreate);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(problemDetails.Errors.ContainsKey("ReviewItems"));
        }
    }
}
