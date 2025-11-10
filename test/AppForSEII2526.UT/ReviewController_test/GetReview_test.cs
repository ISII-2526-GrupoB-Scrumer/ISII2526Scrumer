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
    public class GetReview_test : AppForSEII25264SqliteUT
    {
        public GetReview_test()
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

            var user = new ApplicationUser("Avda. España 1", "600000001", "Manuel", "Garcia", "manolito")
            { Id = "U1" };

            var review = new Review(1, new DateTime(2025, 10, 20))
            {
                Country = "España",
                DriverType = "Habitual",
                Client = user,
                Cars = new List<ReviewItem>()
            };

            var reviewItems = new List<ReviewItem>()
            {
                new ReviewItem(cars[0].Id, "Buen coche para ciudad", 5, review.Id),
                new ReviewItem(cars[1].Id, "Cómodo y eficiente", 4, review.Id)
            };

            review.Cars = reviewItems;

            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(user);
            _context.Add(review);
            _context.AddRange(reviewItems);
            _context.SaveChanges();
        }

        // Test Correcto
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReview_Found_test()
        {
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            var expected = new ReviewDetailDTO(
                1,
                new DateTime(2025, 10, 20),
                "Manuel",
                "España",
                "Habitual",
                new List<ReviewItemDTO>
                {
                    new ReviewItemDTO(1, "Q5", "Audi", "Gasolina", "Blanco", "Buen coche para ciudad", 5),
                    new ReviewItemDTO(2, "Civic", "Honda", "Hibrido", "Azul", "Cómodo y eficiente", 4)
                }
            );


            var result = await controller.GetReview(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var reviewDTOActual = Assert.IsType<ReviewDetailDTO>(okResult.Value);

            Assert.Equal(expected.Id, reviewDTOActual.Id);
            Assert.Equal(expected.Country, reviewDTOActual.Country);
            Assert.Equal(expected.DriverType, reviewDTOActual.DriverType);
            Assert.Equal(expected.Name, reviewDTOActual.Name);
            Assert.Equal(expected.ReviewItems.Count, reviewDTOActual.ReviewItems.Count);

            for (int i = 0; i < expected.ReviewItems.Count; i++)
            {
                Assert.Equal(expected.ReviewItems[i].CarModel, reviewDTOActual.ReviewItems[i].CarModel);
                Assert.Equal(expected.ReviewItems[i].Description, reviewDTOActual.ReviewItems[i].Description);
                Assert.Equal(expected.ReviewItems[i].Rating, reviewDTOActual.ReviewItems[i].Rating);
            }
        }

        // Test Fallo
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReview_NotFound_test()
        {
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            var result = await controller.GetReview(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
