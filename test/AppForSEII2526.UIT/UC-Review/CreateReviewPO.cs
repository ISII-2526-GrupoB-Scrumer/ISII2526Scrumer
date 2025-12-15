using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Review
{
    public class CreateReviewPO : PageObject
    {
        private By inputName = By.Id("Name");
        private By inputClientId = By.Id("ClientId");
        private By inputCountry = By.Id("Country");
        private By selectDriverType = By.Id("Drivertype");
        private By submitButton = By.CssSelector("button[type='submit']");

        public CreateReviewPO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        public void FillReviewerData(string name, string username, string country, string driverType)
        {
            WaitForBeingVisible(inputName);

            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).SendKeys(name);

            _driver.FindElement(inputClientId).Clear();
            _driver.FindElement(inputClientId).SendKeys(username);

            _driver.FindElement(inputCountry).Clear();
            _driver.FindElement(inputCountry).SendKeys(country);

            new SelectElement(_driver.FindElement(selectDriverType))
                .SelectByText(driverType);
        }

        public void FillReviewItemByCarId(int carId, string rating, string description)
        {
            var row = By.Id($"CreateReviewItem_{carId}");
            WaitForBeingVisible(row);

            var rowElement = _driver.FindElement(row);

            _driver.FindElement(By.Id("ratingtxt")).Clear();
            _driver.FindElement(By.Id("ratingtxt")).SendKeys(rating);

            _driver.FindElement(By.Id("descripciontxt")).SendKeys(description);

        }

        public void SubmitReview()
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
