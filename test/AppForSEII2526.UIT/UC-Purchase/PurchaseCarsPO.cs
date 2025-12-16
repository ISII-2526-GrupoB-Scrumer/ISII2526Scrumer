using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit.Abstractions;
using System;

namespace AppForSEII2526.UIT.PageObjects.Purchase
{
    public class PurchaseCarsPO
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;
        protected ITestOutputHelper _output;

        public PurchaseCarsPO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void AddFirstCar()
        {
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            var addButton = _wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath("//table/tbody/tr[1]//button[contains(text(),'Add')]")
                )
            );
            addButton.Click();
        }

        public void Continue()
        {
            var continueButton = _wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(text(),'Continue')]")
                )
            );
            continueButton.Click();
        }
    }
}
