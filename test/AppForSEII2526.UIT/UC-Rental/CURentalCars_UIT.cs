using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Rental;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526
{
    public class CURentalCars_UIT : UC_UIT
    {

        private SelectCarsForRentalPO selectcarsForRental_PO;
        private CreateRentalPO createRental_PO;
        private DetailRentalPO detailRental_PO;

        private const int carId1 = 5;
        private const string carmodel1 = "Corrola";
        private const int carprice1 = 75;
        private const int totalprice1 = 525;
        private const int carId2 = 6;
        private const string carmodel2 = "Civic";
        private const int carprice2 = 120;
        private const int totalprice2 = (120 * 7);


        public CURentalCars_UIT(ITestOutputHelper output) : base(output)
        {


        }



        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "Visa")]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "Google Pay")]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "Paypal")]
        public void UC2_1_2_3(string nombre, string apellido, string id, string direccion, string pago) //Rental correcto 
        {


            //Arrange
            var empieza = DateTime.Now.AddDays(1);
            var termina = DateTime.Now.AddDays(8);


            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");

            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);
            createRental_PO = new CreateRentalPO(_driver, _output);
            detailRental_PO = new DetailRentalPO(_driver, _output);

            //Act
            selectcarsForRental_PO.AddCarToCart(carId1);
            selectcarsForRental_PO.ConfirmRental();

            createRental_PO.FillRentalForm(nombre, apellido, id, direccion, pago);
            createRental_PO.ConfirmRental();
            createRental_PO.ConfirmDialog();

            //Assert
            Assert.True(detailRental_PO.CheckRentalDetail($"{nombre} {apellido}", direccion, pago, totalprice1));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_4() //coches no disponibles
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");

            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);

            selectcarsForRental_PO.SearchCars("peugeot", "1");

            var expected = new List<string[]>();

            Assert.True(selectcarsForRental_PO.CheckBodyTable(expected, By.Id("TableOfRentalItems")));
        }

        [Theory]
        [InlineData("Corrola", "")]
        [InlineData("", "75")]
        [InlineData("Corrola", "75")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_5_6_7(string modelo, string precio) //buscar coches
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");

            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);

            selectcarsForRental_PO.SearchCars(modelo, precio);

            var expected = new List<string[]>
            {
                new[] { "Corrola", "Toyota" }
            };

            Assert.True(selectcarsForRental_PO.CheckBodyTable(expected, By.Id("TableOfRentalItems")));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_8() //coches no seleccionados
        {
            //Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");

            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);


            //Act
            selectcarsForRental_PO.AddCarToCart(carId1);
            selectcarsForRental_PO.RemoveCarFromCart(carId1);

            //Assert

            Assert.True(selectcarsForRental_PO.RentingNotAvailable());

        }


        [Theory]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "Visa")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_9(string nombre,string apellido,string id, string direccion, string pago) //Modificar coches seleccionados
        {
            //Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");

            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);
            createRental_PO = new CreateRentalPO(_driver, _output);
            detailRental_PO = new DetailRentalPO(_driver, _output);

            //Act
            selectcarsForRental_PO.AddCarToCart(carId1);
            selectcarsForRental_PO.AddCarToCart(carId2);
            selectcarsForRental_PO.ConfirmRental();

            

            createRental_PO.FillRentalForm(nombre,apellido,id,direccion,pago);
            createRental_PO.ModifyCars();
            selectcarsForRental_PO.RemoveCarFromCart(carId2);
            selectcarsForRental_PO.ConfirmRental();

            createRental_PO.ConfirmRental();
            createRental_PO.ConfirmDialog();

            //Assert
            Assert.True(detailRental_PO.CheckRentalDetail($"{nombre} {apellido}", direccion, pago, totalprice1));


        }



        [Theory]
        [InlineData("", "Lopez", "U1", "Calle Sol 123, Madrid", "Visa")]
        [InlineData("Carlos", "", "U1", "Calle Sol 123, Madrid", "Visa")]
        [InlineData("Carlos", "Lopez", "", "Calle Sol 123, Madrid", "Visa")]
        [InlineData("Carlos", "Lopez", "U1", "", "Visa")]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_10_11_12_13_14(string nombre, string apellido, string id, string direccion, string pago) //Formulario incompleto
        {
            //Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");
            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);
            createRental_PO = new CreateRentalPO(_driver, _output);

            //Act
            selectcarsForRental_PO.AddCarToCart(carId1);
            selectcarsForRental_PO.ConfirmRental();
            createRental_PO.FillRentalForm(nombre, apellido, id, direccion, pago);
            createRental_PO.ConfirmRental();
            createRental_PO.ConfirmDialog();
            System.Threading.Thread.Sleep(1000);
            //Assert
            Assert.True(createRental_PO.ErrorVisible());

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_15() //Modificar carrito
        {
            //Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");

            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);
            createRental_PO = new CreateRentalPO(_driver, _output);

            //Act
            selectcarsForRental_PO.AddCarToCart(carId1);
            selectcarsForRental_PO.AddCarToCart(carId2);
            selectcarsForRental_PO.RemoveCarFromCart(carId2);

            selectcarsForRental_PO.ConfirmRental();


            var expected = new List<string[]>
            {
                new[] { "Corrola", "Toyota" }
            };

            //Assert
            Assert.True(selectcarsForRental_PO.CheckBodyTable(expected, By.Id("TableOfRentalItems")));


        }

        [Theory]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "Visa")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_Examen(string nombre, string apellido, string id, string direccion, string pago) //Rental correcto 
        {
            //Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "rental/selectcarsforrental");
            Thread.Sleep(2000);
            selectcarsForRental_PO = new SelectCarsForRentalPO(_driver, _output);
            createRental_PO = new CreateRentalPO(_driver, _output);
            detailRental_PO = new DetailRentalPO(_driver, _output);

            //Act
            selectcarsForRental_PO.SearchCars("Civic", ""); //filtro primer coche (honda)
            Thread.Sleep(1000);
            selectcarsForRental_PO.AddCarToCart(carId2); //añado primer coche
            Thread.Sleep(1000);
            selectcarsForRental_PO.SearchCars("", "75"); //filtro segundo coche (corrola)
            Thread.Sleep(1000);
            selectcarsForRental_PO.AddCarToCart(carId1); //añado segundo coche
            Thread.Sleep(1000);
            selectcarsForRental_PO.ConfirmRental();
            Thread.Sleep(1000);
            createRental_PO.ModifyCars(); //voy a create
            Thread.Sleep(1000);
            selectcarsForRental_PO.RemoveCarFromCart(carId2); // quito primer coche (honda)
            Thread.Sleep(1000);

            selectcarsForRental_PO.ConfirmRental(); //vuelvo a create
            Thread.Sleep(1000);

            createRental_PO.FillRentalForm(nombre, apellido, id, direccion, pago);
            Thread.Sleep(1000);
            createRental_PO.ConfirmRental();
            Thread.Sleep(1000);
            createRental_PO.ConfirmDialog();

            //Assert
            Thread.Sleep(1000);
            Assert.True(detailRental_PO.CheckRentalDetail($"{nombre} {apellido}", direccion, pago, totalprice1));

        }
    }
}
