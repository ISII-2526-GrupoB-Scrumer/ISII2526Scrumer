using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526
{
    public class GetCochesParaComprar_test : AppForSEII25264SqliteUT
    {
        public GetCochesParaComprar_test()
        {
            //MODELOS
            var Models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
                new Model("3", "Civic"),
                new Model("4", "Corolla")
            };


            //Coches
            var Cars = new List<Car>()
            {
                //(Id, CarClass, Color, Description, Engine, FuelType, Id, Style, Manufacturer, PurchasingPrice, Doors, Seats, RentingPrice, Consumption, Model)
                new Car("Turimo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 18000, 2, 4, 180, 18, Models[0]),
                new Car("Turimo", "Negro", "", "1.5", "Diesel", 2, "Deportivo", "Peugeot", 15000, 2, 4, 200, 15, Models[1]),
                new Car("Familiar", "Azul", "", "2.0", "Hibrido", 3, "Estandar", "Honda", 22000, 4, 5, 160, 20, Models[2]),
                new Car("Familiar", "Rojo", "", "1.6", "Gasolina", 4, "Estandar", "Toyota", 17000, 4, 5, 140, 17, Models[3])
            };

            //Usuarios
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser("Albacete 1","600000001","Manuel","Garcia","manolito"),
                new ApplicationUser("Albacete 2","600000002","Lucia","Martinez","luci"),
                new ApplicationUser("Albacete 3","600000003","Carlos","Lopez","carlitos"),
                new ApplicationUser("Albacete 4","600000004","Ana","Gonzalez","anita")
            };

            // Se añaden los datos al contexto
            _context.AddRange(Models);
            _context.AddRange(Cars);
            _context.AddRange(users);
            _context.SaveChanges();

        }


        public static IEnumerable<object[]> TestCasesFor_GetCochesParaComprar_OK()
        {
            // DTOs esperados basados en los datos del constructor
            var cocheDTOs = new List<PurchaseSelectDTO>()
            {
                new PurchaseSelectDTO(1, "Q5", "Audi", "Blanco", 18000),
                new PurchaseSelectDTO(2, "508", "Peugeot", "Negro", 15000),
                new PurchaseSelectDTO(3, "Civic", "Honda", "Azul", 22000),
                new PurchaseSelectDTO(4, "Corolla", "Toyota", "Rojo", 17000)
            };

            // Lógica de ordenación del controlador: OrderBy(c.Model.Name).ThenBy(c.PurchasingPrice)

            // Caso 1: sin filtros (null, null)
            var expected1 = cocheDTOs
                .OrderBy(c => c.ModelName)
                .ThenBy(c => c.PurchasingPrice)
                .ToList();

            // Caso 2: filtro por modelo ("Civic", null)
            var expected2 = cocheDTOs
                .Where(c => c.ModelName.Contains("Civic"))
                .OrderBy(c => c.ModelName)
                .ThenBy(c => c.PurchasingPrice)
                .ToList();

            // Caso 3: filtro por color (null, "Blanco")
            var expected3 = cocheDTOs
                .Where(c => c.Color.Contains("Blanco"))
                .OrderBy(c => c.ModelName)
                .ThenBy(c => c.PurchasingPrice)
                .ToList();

            // Caso 4: filtro combinado ("Q5", "Blanco")
            var expected4 = cocheDTOs
               .Where(c => c.ModelName.Contains("Q5") && c.Color.Contains("Blanco"))
               .OrderBy(c => c.ModelName)
               .ThenBy(c => c.PurchasingPrice)
               .ToList();


            var allTests = new List<object[]>
            {
                new object[] { null, null, expected1 },      // sin filtros
                new object[] { "Civic", null, expected2 },   // filtro por modelo
                new object[] { null, "Blanco", expected3 },  // filtro por color
                new object[] { "Q5", "Blanco", expected4 },  // filtro combinado
            };

            return allTests;
        }


        [Theory]
        [MemberData(nameof(TestCasesFor_GetCochesParaComprar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaComprar_OK_test(string? modelo, string? color, IList<PurchaseSelectDTO> expected)
        {
            // Arrange
            var controller = new CarsController(_context, null); // Logger a null para el caso OK

            // Act
            var result = await controller.GetCochesParaCompra(modelo, color);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<PurchaseSelectDTO>>(okResult.Value);

            // Comprobamos que las listas sean iguales (mismo número y orden)
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ModelName, actual[i].ModelName);
                Assert.Equal(expected[i].Manufacturer, actual[i].Manufacturer);
                Assert.Equal(expected[i].Color, actual[i].Color);
                Assert.Equal(expected[i].PurchasingPrice, actual[i].PurchasingPrice);
            }
        }


        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaComprar_ReturnsEmptyList_WhenNoMatch_test()
        {
            // Arrange
            var controller = new CarsController(_context, null);

            // Act
            // Usamos filtros que no devolverán ningún resultado
            var result = await controller.GetCochesParaCompra("ModeloInexistente", "ColorInexistente");

            // Assert
            // A diferencia de GetCochesParaAlquilar, este método no devuelve BadRequest si no hay coches,
            // simplemente devuelve una lista vacía.
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<PurchaseSelectDTO>>(okResult.Value);

            Assert.Empty(actual);
        }
    }
}