/*using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.MaintenanceDTO;
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
    public class GetBooking_test : AppForSEII25264SqliteUT
    {
        public GetBooking_test()
        {
            // ===== MODELOS DE MANTENIMIENTO =====
            var mtTypes = new List<MaintenanceType>()
            {
                new MaintenanceType(1, "Motor"),
                new MaintenanceType(2, "Frenos")
            };

            var maintenances = new List<Maintenance>()
            {
                new Maintenance
                {
                    Id = 1,
                    Name = "Cambio aceite",
                    NumberOfDays = 1,
                    Price = 120,
                    MaintenanceTypes = new List<MaintenanceType>{ mtTypes[0] }
                },
                new Maintenance
                {
                    Id = 2,
                    Name = "Cambio pastillas",
                    NumberOfDays = 1,
                    Price = 90,
                    MaintenanceTypes = new List<MaintenanceType>{ mtTypes[1] }
                }
            };

            // ===== USUARIO =====
            var user = new ApplicationUser("Avda. España 1", "600000001",
                "Manuel", "Garcia", "manolito")
            {
                Id = "U1"
            };

            // ===== BOOKING =====
            var booking = new Booking
            {
                Id = 1,
                Client = user,
                Date = new DateTime(2025, 1, 15),
                PaymentMethod = "Tarjeta",
                ClientId = user.Id,
                Items = new List<BookingItem>()
            };

            booking.Items.Add(new BookingItem
            {
                BookingId = booking.Id,
                MaintenanceID = 1,
                Comment = "Todo correcto",
                Maintenance = maintenances[0]
            });

            booking.Items.Add(new BookingItem
            {
                BookingId = booking.Id,
                MaintenanceID = 2,
                Comment = "Revisar desgaste",
                Maintenance = maintenances[1]
            });

            // ===== GUARDAR EN DB =====
            _context.AddRange(mtTypes);
            _context.AddRange(maintenances);
            _context.Add(user);
            _context.Add(booking);
            _context.SaveChanges();
        }


        // =======================================================
        // TEST 1 - Booking encontrado
        // =======================================================
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBooking_Found_test()
        {
            var logger = new Mock<ILogger<BookingController>>().Object;
            var controller = new BookingController(_context, logger);

            // === Expected DTO (igual a tu controlador) ===
            var expected = new MaintenanceDetailDTO(
                1,
                "U1",
                "Tarjeta",
                new DateTime(2025, 1, 15),
                120 + 90,
                new List<MaintenanceItemDTO>
                {
            new MaintenanceItemDTO(1,"Cambio aceite","Motor",120,1,"Todo correcto"),
            new MaintenanceItemDTO(2,"Cambio pastillas","Frenos",90,1,"Revisar desgaste")
                }
            );

            var result = await controller.GetBooking(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<MaintenanceDetailDTO>(okResult.Value);

            // Comprobaciones principales con un solo Assert
            Assert.Equal(expected, dto); // Compara el DTO completo (expected vs actual)
        }



        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBooking_NotFound_test()
        {
            var logger = new Mock<ILogger<BookingController>>().Object;
            var controller = new BookingController(_context, logger);

            var result = await controller.GetBooking(-1);

            // Expected y actual para el tipo de respuesta
            var expected = typeof(NotFoundResult); // El tipo que esperamos
            var actual = result.GetType(); // El tipo que obtenemos

            // Assert con los parámetros expected y actual
            Assert.Equal(expected, actual); // Compara el tipo esperado con el tipo real
        }

    }
}
*/
