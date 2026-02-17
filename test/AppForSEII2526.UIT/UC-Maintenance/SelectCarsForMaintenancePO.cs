using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526
{
    public class SelectCarsForMaintenancePO : PageObject
    {
        private By inputNombre = By.Id("inputNombre");
        private By inputTipo = By.Id("inputTipo");
        private By buttonBuscar = By.Id("buttonBuscar");
        private By buttonReservar  = By.Id("buttonReservar");

        public SelectCarsForMaintenancePO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchMaintenance(string nombre, string tipo)
        {
            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);
            _driver.FindElement(inputTipo).Clear();
            _driver.FindElement(inputTipo).SendKeys(tipo);
            _driver.FindElement(buttonBuscar).Click();
        }

        public void AddToCart(int id)
        {
            var btnId = By.Id("maint_" + id);
            WaitForBeingClickable(btnId);
            _driver.FindElement(btnId).Click();
        }

        public void RemoveFromCart(int reservaId)
        {
            var btnId = By.Id("remove_" + reservaId);
            WaitForBeingClickable(btnId);
            _driver.FindElement(btnId).Click();
        }

        public void ContinueToBooking()
        {
            WaitForBeingClickable(buttonReservar);
            _driver.FindElement(buttonReservar).Click();
        }

        public bool BookingNotAvailable()
        {
            return _driver.FindElement(buttonReservar).Displayed == false;
        }
    }
}
