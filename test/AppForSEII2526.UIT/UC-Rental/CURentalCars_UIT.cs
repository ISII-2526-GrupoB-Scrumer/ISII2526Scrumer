using AppForSEII2526.UIT.UC_Rental;
using OpenQA.Selenium.DevTools.V137.WebAuthn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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


        public CURentalCars_UIT(ITestOutputHelper output) : base(output)
        {
           

        }



        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData("Carlos","Lopez","U1","Calle Sol 123, Madrid","Visa")]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "Google Pay")]
        [InlineData("Carlos", "Lopez", "U1", "Calle Sol 123, Madrid", "Paypal")]
        public void UC2_1_2_3(string nombre,string apellido,string id, string direccion,string pago)
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
            Assert.True(detailRental_PO.CheckRentalDetail($"{nombre} {apellido}",direccion,pago,DateTime.Today,empieza,termina,totalprice1));
        }



        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_AF1_UC2_11_RentingNotavailable()
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
    }
}
