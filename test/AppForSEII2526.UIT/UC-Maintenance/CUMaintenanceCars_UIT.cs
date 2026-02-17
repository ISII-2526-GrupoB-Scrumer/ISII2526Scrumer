using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Maintenance
{
    public class CUMaintenanceCars_UIT : UC_UIT
    {

        private SelectCarsForMaintenancePO selectPO;
        private CreateMaintenancePO createPO;
        private DetailMaintenancePO detailPO;

        public CUMaintenanceCars_UIT(ITestOutputHelper output) : base(output)
        {

        }

        private const int maintId1 = 1;
        private const string maintName1 = "Cambio de Aceite";
        private const string maintType1 = "Motor";

        [Theory]
        [Trait("LevelTesting", "Functional")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Visa", "Comentario test")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Paypal", "Comentario test")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Google Pay", "Comentario test")]
        public void UC_Maintenance_FullFlow_Success(string nom, string dir, string tel, string pago, string com)
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");

            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);
            detailPO = new DetailMaintenancePO(_driver, _output);

            // Act
            selectPO.AddToCart(1); // Añade el primer mantenimiento disponible [cite: 131]
            selectPO.ContinueToBooking();

            createPO.FillForm(nom, dir, tel, pago, com);
            createPO.ClickContratar();

            // Assert
            Assert.True(detailPO.CheckDetails(nom, dir, ""));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_4() // Esc-2: Servicios no disponibles por filtro [cite: 190]
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);

            selectPO.SearchMaintenance("xxxxx", "xxxxx");

            var expected = new List<string[]>(); // Esperamos tabla vacía [cite: 194]
            Assert.True(selectPO.CheckBodyTable(expected, By.Id("TableOfMaintenances")));
        }

        [Theory]
        [InlineData("Aceite", "")]
        [InlineData("", "Motor")]
        [InlineData("Aceite", "Motor")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_5_6_7(string nombre, string tipo) // Esc-3 Filtrar mantenimientos [cite: 170, 196]
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);

            selectPO.SearchMaintenance(nombre, tipo);
            Thread.Sleep(1000);

            var expected = new List<string[]>
            {
                // Concatenado será "Cambio de Aceite Motor"
                new[] { maintName1, maintType1 }
            };

            Assert.True(selectPO.CheckBodyTable(expected, By.Id("TableOfMaintenances")));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_8() // Esc-4: Servicios no seleccionados [cite: 214]
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);

            selectPO.AddToCart(1);
            selectPO.RemoveFromCart(1);

            Assert.True(_driver.FindElement(By.CssSelector("div.col-4")).Selected == false);
        }

        [Theory]
        [InlineData("", "Calle Sol 123, Madrid", "600000001", "Visa", "Comentario")] // Nombre vacío [cite: 223]
        [InlineData("carlitos_l", "", "600000001", "Visa", "Comentario")]   // Dirección vacía [cite: 238]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "", "Visa", "Comentario")] // Pago vacío [cite: 243]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "", "Comentario")]       // Comentario vacío (específico de este CU) [cite: 99]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Visa", "")]
        [Trait("LevelTesting", "Functional Testing")]

        public void UC3_10_11_12_13_14(string nom, string dir, string tel, string pago, string com) // Esc-6: Error en formulario [cite: 223, 243]
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);

            selectPO.AddToCart(1);
            selectPO.ContinueToBooking();

            createPO.FillForm(nom, dir, tel, pago, com);
            createPO.ClickContratar();

            Assert.True(createPO.ErrorVisible()); // Debe mostrar la alerta de error [cite: 89]
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]

        public void UC3_15() // Esc-7: Modificar servicios desde el formulario [cite: 218]
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);

            // Añadir dos y quitar uno volviendo atrás
            selectPO.AddToCart(1);
            selectPO.ContinueToBooking();

            _driver.FindElement(By.Id("volver")).Click(); // Botón volver [cite: 95]

            selectPO.RemoveFromCart(1);

            Assert.True(selectPO.BookingNotAvailable()); // No se puede continuar sin items [cite: 144]
        }

    }
    
}
