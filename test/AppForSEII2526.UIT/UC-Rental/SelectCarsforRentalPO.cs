using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Rental
{

    internal class SelectCarsforRentalPO : PageObject
    {

        private By inputModelo = By.Id("inputModelo");
        private By inputPrecio = By.Id("inputPrecio");
        private By buttonBuscar = By.Id("buttonBuscar");
        
        protected SelectCarsforRentalPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarCoches(string modelo,int precio)
        {
            WaitForBeingClickable(inputModelo);
            _driver.FindElement(inputModelo).Clear();
            _driver.FindElement(inputModelo).Click();
            _driver.FindElement(inputModelo).SendKeys(modelo);
            _driver.FindElement(buttonBuscar).Click();
        }

        public void AddCarToCart(string modelo)
        {
            WaitForBeingClickable(By.Id("carToRent_" + modelo));

            _driver.FindElement(By.Id("carToRent_" + modelo)).Click();
        }

        public void RemoveCarFromCart(string modelo)
        {
            WaitForBeingClickable(By.Id("removeCar_" + modelo));
            _driver.FindElement(By.Id("removeCar_" + modelo)).Click();
        }

        public bool RentingNotAvailable()
        {

            return _driver.FindElement(buttonBuscar).Displayed == false;
        }

    }
}
