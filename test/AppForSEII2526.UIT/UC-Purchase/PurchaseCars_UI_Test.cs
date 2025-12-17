using AppForSEII2526.UIT.PageObjects.Purchase;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class PurchaseCars_UI_Test : UC_UIT
    {
        private readonly WebDriverWait _wait;

        public PurchaseCars_UI_Test(ITestOutputHelper output)
            : base(output)
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }

        [Fact]
        public void UC1_1_VisaSuccessfulPurchase_FlowWorksCorrectly()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Visa",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            Assert.Contains("/purchase/details/", _driver.Url);
        }

        [Fact]
        public void UC1_2_SuccessfulPurchase_WithMasterCard_FlowWorksCorrectly()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "MasterCard",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            Assert.Contains("/purchase/details/", _driver.Url);
        }

        [Fact]
        public void UC1_3_SuccessfulPurchase_WithPayPal_FlowWorksCorrectly()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "PayPal",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            Assert.Contains("/purchase/details/", _driver.Url);
        }

        [Fact]
        public void UC1_4_SuccessfulPurchase_WithBankTransfer_FlowWorksCorrectly()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Bank Transfer",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            Assert.Contains("/purchase/details/", _driver.Url);
        }

        [Fact]
        public void UC1_5_FilterByColor_Rojo_DisplaysOnlyRojoCars()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']")
            ));
            colorInput.SendKeys("Rojo");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            var table = _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));

            Assert.Contains("Rojo", table.Text);
            Assert.DoesNotContain("Blanco", table.Text);
            Assert.DoesNotContain("Azul", table.Text);
        }

        [Fact]
        public void UC1_6_FilterByModel_Ferrari_DisplaysNoResults()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Ferrari");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            Assert.DoesNotContain("Ferrari", table.Text);
            Assert.Contains("No results found", table.Text);
        }

        [Fact]
        public void UC1_7_FilterByColor_Morado_DisplaysNoResults()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']")
            ));
            colorInput.SendKeys("Morado");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            Assert.DoesNotContain("Morado", table.Text);
            Assert.Contains("No results found", table.Text);
        }

        [Fact]
        public void UC1_8_FilterByModel_Civic_DisplaysOnlyCivic()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Civic");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Search')]")));

            var table = _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));

            Assert.Contains("Civic", table.Text);
            Assert.DoesNotContain("Corrola", table.Text);
            Assert.DoesNotContain("Escape", table.Text);
        }

        [Fact]
        public void UC1_9_FilterByModelAndColor_DisplaysOnlyMatchingCars()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Civic");

            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']")
            ));
            colorInput.SendKeys("Rojo");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            Assert.Contains("Civic", table.Text);
            Assert.Contains("Rojo", table.Text);
            Assert.DoesNotContain("Azul", table.Text);
            Assert.DoesNotContain("Blanco", table.Text);
        }

        [Fact]
        public void UC1_10_FilterByModelAndColor_DisplaysNoResultsForColor()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Civic");

            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']")
            ));
            colorInput.SendKeys("Morado");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            Assert.DoesNotContain("Civic", table.Text);
            Assert.Contains("No results found", table.Text);
        }

        [Fact]
        public void UC1_11_EmptyDeliveryDealer_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            FillPurchaseForm(
                delivery: null,
                payment: "Visa",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var error = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.alert.alert-danger")));
            Assert.Contains("Delivery location is required", error.Text);
        }

        [Fact]
        public void UC1_12_EmptyPaymentMethod_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: null,
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var error = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Payment')]")));
            Assert.True(error.Displayed);
        }

        [Fact]
        public void UC1_13_EmptyDriverType_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Visa",
                driver: null,
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var error = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Driver')]")));
            Assert.True(error.Displayed);
        }

        [Fact]
        public void UC1_14_EmptyPurchaseDate_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Visa",
                driver: "Experienced",
                country: "Spain",
                date: null
            );

            var error = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Date')]")));
            Assert.True(error.Displayed);
        }

        private void GoToCreatePurchaseWithOneCar()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();
        }

        private void FillPurchaseForm(
            string? delivery,
            string? payment,
            string? driver,
            string? country,
            DateTime? date)
        {
            if (!string.IsNullOrEmpty(delivery))
                _driver.FindElement(By.CssSelector("input[placeholder='e.g. Madrid Center']"))
                       .SendKeys(delivery);

            var selects = _driver.FindElements(By.CssSelector("select.form-select"));

            if (!string.IsNullOrEmpty(payment))
                new SelectElement(selects[0]).SelectByText(payment);

            if (!string.IsNullOrEmpty(driver))
                new SelectElement(selects[1]).SelectByText(driver);

            if (!string.IsNullOrEmpty(country))
                _driver.FindElement(By.CssSelector("input[placeholder='e.g. Spain']"))
                       .SendKeys(country);

            if (date.HasValue)
                _driver.FindElement(By.CssSelector("input[type='date']"))
                       .SendKeys(date.Value.ToString("yyyy-MM-dd"));

            _driver.FindElement(By.XPath("//button[contains(text(),'Confirm')]")).Click();
        }
    }
}