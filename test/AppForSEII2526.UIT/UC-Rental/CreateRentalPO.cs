using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Rental
{
    public class CreateRentalPO : PageObject
    {

        private By inputName = By.Id("Name");
        private By inputSurname = By.Id("Surname");
        private By inputClientId = By.Id("ClientId");
        private By inputAddress = By.Id("DeliveryAddress");
        private By paymentMethod = By.Id("PaymentMethod");
        private By submitButton = By.Id("Submit");

        public CreateRentalPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillRentalForm(string name, string surname, string clientId, string address, string paymentMethodValue)
        {

            WaitForBeingVisible(By.Id("Name"));

            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).Click();
            _driver.FindElement(inputName).SendKeys(name);

            _driver.FindElement(inputSurname).Clear();
            _driver.FindElement(inputSurname).Click();
            _driver.FindElement(inputSurname).SendKeys(surname);

            _driver.FindElement(inputClientId).Clear();
            _driver.FindElement(inputClientId).Click();
            _driver.FindElement(inputClientId).SendKeys(clientId);

            _driver.FindElement(inputAddress).Clear();
            _driver.FindElement(inputAddress).Click();
            _driver.FindElement(inputAddress).SendKeys(address);

            new SelectElement(_driver.FindElement(paymentMethod)).SelectByText(paymentMethodValue);

            
        }

        public void ConfirmRental()
        {
            WaitForBeingClickable(submitButton);
            _driver.FindElement(submitButton).Click();

        }

        public void ConfirmDialog()
        {
            PressOkModalDialog();
        }
    }
}
