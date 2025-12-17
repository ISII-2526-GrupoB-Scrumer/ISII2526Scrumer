using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AppForSEII2526
{
    public class SelectCarsForRentalPO : PageObject
    {

        private By inputModelo = By.Id("inputModelo");
        private By inputPrecio = By.Id("inputPrecio");
        private By buttonBuscar = By.Id("buttonBuscar");
        private By buttonConfirmar = By.Id("buttonConfirmar");

        
        public SelectCarsForRentalPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void SearchCars(string modelo, string precio)
        {


            _driver.FindElement(inputModelo).Clear();
            _driver.FindElement(inputModelo).Click();
            _driver.FindElement(inputModelo).SendKeys(modelo);

            _driver.FindElement(inputPrecio).Clear();
            _driver.FindElement(inputPrecio).Click();
            _driver.FindElement(inputPrecio).SendKeys(precio);

            _driver.FindElement(buttonBuscar).Click();
        }

        public void AddCarToCart(int id)
        {
            WaitForBeingClickable(By.Id("carToRent_" + id));

            _driver.FindElement(By.Id("carToRent_" + id)).Click();
        }

        public void RemoveCarFromCart(int id)
        {
            WaitForBeingClickable(By.Id("removeCar_" + id));

            _driver.FindElement(By.Id("removeCar_" + id)).Click();
        }

        public bool RentingNotAvailable()
        {
            return _driver.FindElement(buttonConfirmar).Displayed == false;
        }

        public void ConfirmRental()
        {
            WaitForBeingClickable(buttonConfirmar);
            _driver.FindElement(buttonConfirmar).Click();
        }

    }
}
