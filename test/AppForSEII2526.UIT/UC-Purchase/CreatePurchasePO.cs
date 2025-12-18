using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit.Abstractions;
using System;

namespace AppForSEII2526.UIT.PageObjects.Purchase
{
    public class CreatePurchasePO
    {
        private By modifyButton = By.Id("ModifyCars");
        protected IWebDriver _driver;
        protected WebDriverWait _wait;
        protected ITestOutputHelper _output;

        public CreatePurchasePO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void FillForm(
            string deliveryDealer,
            string paymentMethod,
            string driverType,
            string country,
            DateTime purchaseDate)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='e.g. Madrid Center']")
            ));

            _driver.FindElement(By.CssSelector("input[placeholder='e.g. Madrid Center']"))
                   .SendKeys(deliveryDealer);

            var selects = _driver.FindElements(By.CssSelector("select.form-select"));
            new SelectElement(selects[0]).SelectByText(paymentMethod);
            new SelectElement(selects[1]).SelectByText(driverType);

            _driver.FindElement(By.CssSelector("input[placeholder='e.g. Spain']"))
                   .SendKeys(country);

            var dateInput = _driver.FindElement(By.CssSelector("input[type='date']"));
            dateInput.SendKeys(purchaseDate.ToString("yyyy-MM-dd"));
        }

        public void Confirm()
        {
            var confirmButton = _wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(text(),'Confirm Purchase')]")
                )
            );
            confirmButton.Click();
        }
        public void ModifyCars()
        {
           
            _driver.FindElement(modifyButton).Click();
        }
    }
}
