using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526
{
    public class PostRental_test : AppForSEII25264SqliteUT
    {
        public PostRental_test()
        {
            // === Modelos ===
            var models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "Civic"),
            };

            // === Coches ===
            var cars = new List<Car>()
            {
                new Car("Turismo","Blanco","","1.8","Gasolina",1,"Estandar","Audi",18000,2,4,180,18,models[0]),
                new Car("Familiar","Azul","","2.0","Hibrido",2,"Estandar","Honda",22000,4,5,160,20,models[1])
            };

            // === Usuario ===
            var user = new ApplicationUser("Avda. España 1", "600000001", "Manuel", "Garcia", "manolito")
            {
                Id = "U1"
            };

            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(user);
            _context.SaveChanges();
        }

        // OK
        // OK
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateRental_OK_test()
        {
            // Arrange
            var logger = new Mock<ILogger<RentalController>>().Object;
            var controller = new RentalController(_context, logger);

            var rentalCreate = new RentalCreateDTO
            {
                StartDate = new DateTime(2026, 1, 10),
                EndDate = new DateTime(2026, 1, 12),
                PaymentMethod = "Visa",
                DeliveryCarDealer = "Concesionario Albacete",
                ClientId = "U1",
                RentalItems = new List<RentalItemDTO>
        {
            new RentalItemDTO(1, "Q5", 180m, 2, "Audi"),
            new RentalItemDTO(2, "Civic", 160m, 1, "Honda")
        }
            };

            // Act
            var result = await controller.CreateRental(rentalCreate);

            // Assert
            var createdResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode); // Created

            var dto = Assert.IsType<RentalDetailDTO>(createdResult.Value);

            
            var expected = new RentalCreateDTO
            {
                ClientId = "U1",
                Name = "Manuel",
                Surname = "Garcia",
                Address = "Avda. España 1",
                StartDate = rentalCreate.StartDate,
                EndDate = rentalCreate.EndDate,
                PaymentMethod = rentalCreate.PaymentMethod,
                TotalPrice = dto.TotalPrice,   
                RentalItems = rentalCreate.RentalItems
            };

            // Comparamos usando SOLO tu Equals()
            Assert.Equal(expected, dto);
        }


        // BAD REQUEST
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateRental_BadRequest_test()
        {
            // Arrange
            var logger = new Mock<ILogger<RentalController>>().Object;
            var controller = new RentalController(_context, logger);

            var rentalCreate = new RentalCreateDTO
            {
                StartDate = new DateTime(2026, 1, 10),
                EndDate = new DateTime(2026, 1, 12),
                PaymentMethod = "Visa",
                ClientId = "U0", 
                RentalItems = new List<RentalItemDTO>() 
            };

            // Act
            var result = await controller.CreateRental(rentalCreate);

            // Assert
            var badRequest = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode); // Bad Request

            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
            Assert.True(problemDetails.Errors.ContainsKey("Client") ||
                        problemDetails.Errors.ContainsKey("RentalItems"));
        }
    }
}
