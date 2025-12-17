using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.MaintenanceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AppForSEII2526
{
    public class GetMaintenances_test : AppForSEII25264SqliteUT
    {
        public GetMaintenances_test()
        {
            // ===== Modelos de tipo =====
            var tipos = new List<MaintenanceType>()
            {
                new MaintenanceType(1, "Motor"),
                new MaintenanceType(2, "Transmisión"),
                new MaintenanceType(3, "Frenos"),
            };

            // ===== Mantenimientos =====
            var mantenimientos = new List<Maintenance>()
            {
                new Maintenance
                {
                    Id = 1,
                    Name = "Cambio Aceite",
                    Price = 80m,
                    NumberOfDays = 1,
                    MaintenanceTypes = new List<MaintenanceType>() { tipos[0] }
                },
                new Maintenance
                {
                    Id = 2,
                    Name = "Revisión General",
                    Price = 150m,
                    NumberOfDays = 2,
                    MaintenanceTypes = new List<MaintenanceType>() { tipos[1] }
                },
                new Maintenance
                {
                    Id = 3,
                    Name = "Cambio Pastillas",
                    Price = 120m,
                    NumberOfDays = 1,
                    MaintenanceTypes = new List<MaintenanceType>() { tipos[2] }
                }
            };

            _context.AddRange(tipos);
            _context.AddRange(mantenimientos);
            _context.SaveChanges();
        }

        // =====================================================
        //   CASOS DE PRUEBA (OK)
        // =====================================================
        public static IEnumerable<object[]> TestCases_GetMaintenances_OK()
        {
            var data = new List<MaintenanceSelectDTO>()
            {
                new MaintenanceSelectDTO(1, "Cambio Aceite", "Motor", 80m, 1),
                new MaintenanceSelectDTO(2, "Revisión General", "Transmisión", 150m, 2),
                new MaintenanceSelectDTO(3, "Cambio Pastillas", "Frenos", 120m, 1),
            };

            var expected1 = data.OrderBy(m => m.Name).ThenBy(m => m.Price).ToList();

            var expected2 = data
                .Where(m => m.Name.Contains("Cambio"))
                .OrderBy(m => m.Name)
                .ThenBy(m => m.Price)
                .ToList();

            var expected3 = data
                .Where(m => m.Type.Contains("Motor"))
                .OrderBy(m => m.Name)
                .ThenBy(m => m.Price)
                .ToList();

            var expected4 = data
                .Where(m => m.Name.Contains("Cambio") && m.Type.Contains("Motor"))
                .OrderBy(m => m.Name)
                .ThenBy(m => m.Price)
                .ToList();

            return new List<object[]>
            {
                new object[] { null, null, expected1 },
                new object[] { "Cambio", null, expected2 },
                new object[] { null, "Motor", expected3 },
                new object[] { "Cambio", "Motor", expected4 }
            };
        }

        // =====================================================
        //   TEST OK
        // =====================================================
        [Theory]
        [MemberData(nameof(TestCases_GetMaintenances_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaintenances_OK_test(string? nombre, string? tipo, IList<MaintenanceSelectDTO> expected)
        {
            var controller = new MaintenanceController(_context, null);

            var result = await controller.GetMaintenances(nombre, tipo);

            var ok = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<MaintenanceSelectDTO>>(ok.Value);

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Type, actual[i].Type);
                Assert.Equal(expected[i].Price, actual[i].Price);
                Assert.Equal(expected[i].NumberOfDays, actual[i].NumberOfDays);
            }
        }

        // =====================================================
        //   TEST: Lista vacía (no es error)
        // =====================================================
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaintenances_EmptyList_test()
        {
            var controller = new MaintenanceController(_context, null);

            var result = await controller.GetMaintenances("NoExiste", null);

            var ok = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<MaintenanceSelectDTO>>(ok.Value);

            Assert.Empty(actual);
        }
    }
}
