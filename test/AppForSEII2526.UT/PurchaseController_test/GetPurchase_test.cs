using AppForSEII2526.API;
using AppForSEII2526;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526
{
    public class GetPurchase_test : AppForSEII25264SqliteUT
    {
        public GetPurchase_test()
        {
            // ====== Datos base ======
            var models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
            };

            var cars = new List<Car>()
            {
                new Car("Turismo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 20000, 2, 4, 180, 18, models[0]),
                new Car("Turismo","Negro","","1.5","Diesel", 2,"Deportivo","Peugeot", 15000, 2, 4, 200, 15, models[1])
            };

            var user = new ApplicationUser("Albacete 1", "600000001", "Manuel", "Garcia", "manolito") { Id = "U1" };

            // ====== Purchase ======
            var purchase = new Purchase
            {
                Id = 1,
                Client = user,
                DeliveryCarDealer = "Albacete Center",
                PurchasingDate = DateTime.Now,
                PaymentMethod = "Visa",
                DriverType = "Experienced",
                Country = "Spain",
                PurchaseItems = new List<PurchaseItem>()
            };

            purchase.PurchaseItems.Add(new PurchaseItem
            {
                Car = cars[0], // Audi Q5
                Quantity = 2
            });

            purchase.PurchasingPrice = purchase.PurchaseItems.Sum(pi => pi.Car.PurchasingPrice * pi.Quantity); // 40000

            // ====== Guardar en DB ======
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(user);
            _context.Add(purchase);
            _context.SaveChanges();
        }

        //PURCHASE NO ENCONTRADO
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_NotFound_test()
        {
            // Arrange
            var logger = new Mock<ILogger<PurchaseController>>().Object;
            var controller = new PurchaseController(_context, logger);

            // Act
            var result = await controller.GetPurchase(-1); // ID inexistente

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //PURCHASE ENCONTRADO
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_Found_test()
        {
            // Arrange
            var logger = new Mock<ILogger<PurchaseController>>().Object;
            var controller = new PurchaseController(_context, logger);

            // ====== Expected DTO (basado en el constructor) ======
            var expectedPurchase = new PurchaseDetailDTO(
                1,
                DateTime.Now, // Fecha (aproximada)
                "Visa",
                "Experienced",
                "Albacete Center",
                "Spain",
                // --- CORRECCIÓN ---
                // La variable en el DTO debe coincidir con la pasada por el controlador
                "manolito", // ClientUserName
                            // --- FIN CORRECCIÓN ---
                40000m,
                new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(1, "Q5", 20000m, 2)
                }
            );

            // ====== Act ======
            var result = await controller.GetPurchase(1); // ID existente

            // ====== Assert ======
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDTO = Assert.IsType<PurchaseDetailDTO>(okResult.Value);

            // Verificar campos principales
            Assert.Equal(expectedPurchase.Id, actualDTO.Id);
            Assert.Equal(expectedPurchase.DeliveryCarDealer, actualDTO.DeliveryCarDealer);
            Assert.Equal(expectedPurchase.PurchasingPrice, actualDTO.PurchasingPrice);
            // --- CORRECCIÓN ---
            Assert.Equal(expectedPurchase.ClientId, actualDTO.ClientId);
            // --- FIN CORRECCIÓN ---

            // Verificar que los PurchaseItems coinciden
            Assert.Single(actualDTO.PurchaseItems);
            var expectedItem = expectedPurchase.PurchaseItems.First();
            var actualItem = actualDTO.PurchaseItems.First();

            Assert.Equal(expectedItem.CarId, actualItem.CarId);
            Assert.Equal(expectedItem.CarModel, actualItem.CarModel);
            Assert.Equal(expectedItem.PurchasingPrice, actualItem.PurchasingPrice);
            Assert.Equal(expectedItem.Quantity, actualItem.Quantity);
        }
    }
}