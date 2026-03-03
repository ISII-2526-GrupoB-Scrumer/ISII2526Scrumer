using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private const string maintName1 = "Cambio aceite";
        private const string maintType1 = "Motor";

        [Theory]
        [Trait("LevelTesting", "Functional")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Visa", "Comentario test", "100")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Paypal", "Comentario test", "100")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Google Pay", "Comentario test", "100")]
        public void UC3_1_2_3(string nom, string dir, string tel, string pago, string com, string precioEsperado)
        {
            // 1. Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");

            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);
            detailPO = new DetailMaintenancePO(_driver, _output);

            // 2. Act
            selectPO.AddToCart(1);
            selectPO.ContinueToBooking();

            createPO.FillForm(nom, dir, tel, pago, com);
            createPO.ClickContratar();

            // 3. Assert
            Assert.True(detailPO.CheckDetails(nom, dir, precioEsperado), "Los datos generales o el precio no coinciden.");
            Assert.True(_driver.PageSource.Contains("Cambio aceite"), "El servicio 'Cambio aceite' no aparece en el detalle.");
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_4()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);

            selectPO.SearchMaintenance("xxxxx", "xxxxx");

            var expected = new List<string[]>();
            Assert.True(selectPO.CheckBodyTable(expected, By.Id("TableOfMaintenances")));
        }

        [Theory]
        [InlineData("Aceite", "")]
        [InlineData("", "Motor")]
        [InlineData("Aceite", "Motor")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_5_6_7(string nombre, string tipo)
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);

            selectPO.SearchMaintenance(nombre, tipo);
            Thread.Sleep(1000);

            var expected = new List<string[]>
            {

                new[] { maintName1, maintType1 }
            };

            Assert.True(selectPO.CheckBodyTable(expected, By.Id("TableOfMaintenances")));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_8()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);

            selectPO.AddToCart(1);
            selectPO.RemoveFromCart(1);

            Assert.True(_driver.FindElement(By.CssSelector("div.col-4")).Selected == false);
        }

        [Theory]
        [InlineData("", "Calle Sol 123, Madrid", "600000001", "Visa", "Comentario")]
        [InlineData("carlitos_l", "", "600000001", "Visa", "Comentario")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "", "Comentario")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Visa", "")]
        [Trait("LevelTesting", "Functional Testing")]

        public void UC3_9_10_11_12(string nom, string dir, string tel, string pago, string com)
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);

            selectPO.AddToCart(1);
            selectPO.ContinueToBooking();

            createPO.FillForm(nom, dir, tel, pago, com);
            createPO.ClickContratar();

            Assert.True(createPO.ErrorVisible());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]

        public void UC3_13()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);

            // Añadir dos y quitar uno volviendo atrás
            selectPO.AddToCart(1);
            selectPO.ContinueToBooking();

            _driver.FindElement(By.Id("volver")).Click();

            selectPO.RemoveFromCart(1);

            Assert.True(selectPO.BookingNotAvailable());
        }



        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Visa", "comentario","400")]
        public void UC3_14(string nom, string dir, string tel, string pago, string com,string precioEsperado)
        {
            // 1. Arrange: Abrir página y preparar POs
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");

            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);
            detailPO = new DetailMaintenancePO(_driver, _output);

            // 2. Act: Seleccionar primer mantenimiento y pasar al Create
            selectPO.AddToCart(1); // Añade id=1
            selectPO.ContinueToBooking();
            Thread.Sleep(1000); // Espera a que cargue la página de Create

            // 3. Act: Rellenar datos iniciales
            createPO.FillForm(nom, dir, tel, pago, com);
            Thread.Sleep(500);

            // 4. Act: Volver al Select (botón "volver")
            _driver.FindElement(By.Id("volver")).Click();
            Thread.Sleep(1000);

            // 5. Act: Seleccionar un segundo mantenimiento (id=2)
            selectPO.AddToCart(2);
            selectPO.ContinueToBooking();
            Thread.Sleep(1000);

            // 6. Act: Finalizar la contratación
            createPO.FillForm(nom, dir, tel, pago, com);
            createPO.ClickContratar();
            Thread.Sleep(1000);

            // 7. Assert: Verificar en el detalle que aparecen los datos y el precio total
            Assert.True(detailPO.CheckDetails(nom, dir, precioEsperado), "Los datos generales o el precio no coinciden.");
            Assert.True(_driver.PageSource.Contains("Cambio aceite"), "El servicio 'Cambio aceite' no aparece en el detalle.");
        }

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData("carlitos_l", "Calle Sol 123, Madrid", "600000001", "Visa", "comentario", "250")]
        public void UC3_15(string nom, string dir, string tel, string pago, string com, string precioEsperado)
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "maintenance/selectcarsformaintenance");
            selectPO = new SelectCarsForMaintenancePO(_driver, _output);
            createPO = new CreateMaintenancePO(_driver, _output);
            detailPO = new DetailMaintenancePO(_driver, _output);

            selectPO.SearchMaintenance("", "Motor");
            selectPO.AddToCart(1);
            Thread.Sleep(1000);
            selectPO.SearchMaintenance("Revision completa", "");
            selectPO.AddToCart(6);
            Thread.Sleep(1000);

            //borro id 2
            selectPO.RemoveFromCart(1);
            Thread.Sleep(1000);

            selectPO.ContinueToBooking();
            createPO.FillForm(nom, dir, tel, pago, com);
            createPO.ClickContratar();

            Assert.True(detailPO.CheckDetails(nom, dir, precioEsperado), "Los datos generales o el precio no coinciden.");
            Assert.True(_driver.PageSource.Contains("Revision completa"), "El servicio 'Revision completa' no aparece en el detalle. Es urgente");
        }
    }
}