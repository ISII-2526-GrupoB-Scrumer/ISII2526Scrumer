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
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526
{
    public class PostPurchase_test : AppForSEII25264SqliteUT
    {
        public PostPurchase_test()
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
                new Car("Turismo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 20000, 2, 4, 180, 18, models[0]),
                new Car("Familiar","Azul","","2.0","Hibrido", 2,"Estandar","Honda", 15000, 4, 5, 160, 20, models[1])
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
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_OK_test()
        {
            // Arrange
            var logger = new Mock<ILogger<PurchaseController>>().Object;
            var controller = new PurchaseController(_context, logger);

            var purchaseCreate = new PurchaseCreateDTO
            {
                PurchasingDate = DateTime.Now,
                PaymentMethod = "Visa",
                DriverType = "Experienced",
                DeliveryCarDealer = "Concesionario Albacete",
                Country = "Spain",
                ClientId = "U1",
                PurchaseItems = new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(1, "Q5", 20000m, 2), // 40000
                    new PurchaseItemDTO(2, "Civic", 15000m, 1)  // 15000
                }
            };
            // Precio total esperado = 55000

            // Act
            var result = await controller.CreatePurchase(purchaseCreate);

            // Assert
            var createdResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode); // 201 Created

            var dto = Assert.IsType<PurchaseDetailDTO>(createdResult.Value);

            // Comprobación única de todos los datos relevantes en un solo assert
            var expected = (
                PaymentMethod: purchaseCreate.PaymentMethod,
                DeliveryCarDealer: purchaseCreate.DeliveryCarDealer,
                ClientId: "manolito", // El DTO devuelve el user name
                TotalQuantity: 3,
                PurchasingPrice: 55000m
            );

            var actual = (
                PaymentMethod: dto.PaymentMethod,
                DeliveryCarDealer: dto.DeliveryCarDealer,
                ClientId: dto.ClientId,
                TotalQuantity: dto.PurchaseItems.Sum(i => i.Quantity),
                PurchasingPrice: dto.PurchasingPrice
            );

            Assert.Equal(expected, actual);
        }

        // BAD REQUEST
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_test()
        {
            // Arrange
            var logger = new Mock<ILogger<PurchaseController>>().Object;
            var controller = new PurchaseController(_context, logger);

            // Caso 1: Usuario no existe
            var purchaseBadUser = new PurchaseCreateDTO
            {
                ClientId = "U99", // Usuario inexistente
                PurchaseItems = new List<PurchaseItemDTO>
                {
                     new PurchaseItemDTO(1, "Q5", 20000m, 1)
                }
            };

            // Caso 2: Sin items
            var purchaseNoItems = new PurchaseCreateDTO
            {
                ClientId = "U1",
                PurchaseItems = new List<PurchaseItemDTO>() // Lista vacía
            };

            // Act
            var resultUser = await controller.CreatePurchase(purchaseBadUser);

            // Crear nueva instancia del controlador para aislar ModelState entre peticiones
            var controllerForItems = new PurchaseController(_context, logger);
            var resultItems = await controllerForItems.CreatePurchase(purchaseNoItems);

            // Assert (Bad User) — una sola aserción que verifica código y keys de error
            var badRequestUser = Assert.IsAssignableFrom<ObjectResult>(resultUser);
            var problemDetailsUser = Assert.IsType<ValidationProblemDetails>(badRequestUser.Value);
            var actualUser = $"{badRequestUser.StatusCode ?? 0}:{string.Join(',', problemDetailsUser.Errors.Keys.OrderBy(k => k))}";
            var expectedUser = "400:ClientId";
            Assert.Equal(expectedUser, actualUser);

            // Assert (No Items) — una sola aserción que verifica código y keys de error
            var badRequestItems = Assert.IsAssignableFrom<ObjectResult>(resultItems);
            var problemDetailsItems = Assert.IsType<ValidationProblemDetails>(badRequestItems.Value);
            var actualItems = $"{badRequestItems.StatusCode ?? 0}:{string.Join(',', problemDetailsItems.Errors.Keys.OrderBy(k => k))}";
            var expectedItems = "400:PurchaseItems";
            Assert.Equal(expectedItems, actualItems);
        }
    }
}