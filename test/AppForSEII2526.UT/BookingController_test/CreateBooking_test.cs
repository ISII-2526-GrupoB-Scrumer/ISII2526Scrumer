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
    public class CreateBooking_test : AppForSEII25264SqliteUT
    {
        public CreateBooking_test()
        {
            // ===== TIPOS =====
            var mtTypes = new List<MaintenanceType>()
            {
                new MaintenanceType(1, "Motor"),
                new MaintenanceType(2, "Frenos")
            };

            // ===== MANTENIMIENTOS =====
            var maintenances = new List<Maintenance>()
            {
                new Maintenance
                {
                    Id = 1,
                    Name = "Cambio aceite",
                    Price = 120,
                    NumberOfDays = 1,
                    MaintenanceTypes = new List<MaintenanceType>{ mtTypes[0] }
                },
                new Maintenance
                {
                    Id = 2,
                    Name = "Cambio pastillas",
                    Price = 90,
                    NumberOfDays = 1,
                    MaintenanceTypes = new List<MaintenanceType>{ mtTypes[1] }
                }
            };

            // ===== USUARIO =====
            var user = new ApplicationUser("Avda. España 1", "600000001",
                "Manuel", "Garcia", "manolito")
            {
                Id = "U1"
            };

            // GUARDAR EN DB
            _context.AddRange(mtTypes);
            _context.AddRange(maintenances);
            _context.Add(user);
            _context.SaveChanges();
        }

        // =======================================================
        // TEST 1 - CreateBooking OK
        // =======================================================
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBooking_OK_test()
        {
            var logger = new Mock<ILogger<BookingController>>().Object;
            var controller = new BookingController(_context, logger);

            var bookingCreate = new MaintenanceCreateDTO
            {
                ClientId = "manolito",
                PaymentMethod = "Tarjeta",
                MaintenanceItems = new List<MaintenanceItemDTO>
                {
                    new MaintenanceItemDTO(1,"Cambio aceite","Motor",120,1,"Todo correcto"),
                    new MaintenanceItemDTO(2,"Cambio pastillas","Frenos",90,1,"Revisar desgaste")
                }
            };

            var result = await controller.CreateBooking(bookingCreate);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<MaintenanceDetailDTO>(created.Value);

            // Validaciones principales
            Assert.Equal("manolito", dto.ClientId);
            Assert.Equal("Tarjeta", dto.PaymentMethod);

            // Total esperado = 120 + 90
            Assert.Equal(210, dto.TotalPrice);

            // Validación de items
            Assert.Equal(2, dto.Maintenances.Count);

            Assert.Equal(1, dto.Maintenances[0].MaintenanceId);
            Assert.Equal(2, dto.Maintenances[1].MaintenanceId);
        }

        // =======================================================
        // TEST 2 - CreateBooking BAD REQUEST (usuario no existe)
        // =======================================================
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBooking_BadRequest_UserNotFound_test()
        {
            var logger = new Mock<ILogger<BookingController>>().Object;
            var controller = new BookingController(_context, logger);

            var bookingCreate = new MaintenanceCreateDTO
            {
                ClientId = "UsuarioQueNoExiste",
                PaymentMethod = "Tarjeta",
                MaintenanceItems = new List<MaintenanceItemDTO>
                {
                    new MaintenanceItemDTO(1,"Cambio aceite","Motor",120,1,"OK")
                }
            };

            var result = await controller.CreateBooking(bookingCreate);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(details.Errors.ContainsKey("Client"));
        }

        // =======================================================
        // TEST 3 - CreateBooking BAD REQUEST (lista vacía)
        // =======================================================
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBooking_BadRequest_EmptyList_test()
        {
            var logger = new Mock<ILogger<BookingController>>().Object;
            var controller = new BookingController(_context, logger);

            var bookingCreate = new MaintenanceCreateDTO
            {
                ClientId = "manolito",
                PaymentMethod = "Tarjeta",
                MaintenanceItems = new List<MaintenanceItemDTO>() // VACÍO
            };

            var result = await controller.CreateBooking(bookingCreate);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(details.Errors.ContainsKey("MaintenanceItems"));
        }

        // =======================================================
        // TEST 4 - CreateBooking BAD REQUEST (mantenimiento no existe)
        // =======================================================
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBooking_BadRequest_MaintenanceNotFound_test()
        {
            var logger = new Mock<ILogger<BookingController>>().Object;
            var controller = new BookingController(_context, logger);

            var bookingCreate = new MaintenanceCreateDTO
            {
                ClientId = "manolito",
                PaymentMethod = "Tarjeta",
                MaintenanceItems = new List<MaintenanceItemDTO>
                {
                    new MaintenanceItemDTO(999,"Invalido","N/A",0,0,"Error") // NO EXISTE
                }
            };

            var result = await controller.CreateBooking(bookingCreate);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(details.Errors.ContainsKey("MaintenanceItems"));
        }
    }
}
*/