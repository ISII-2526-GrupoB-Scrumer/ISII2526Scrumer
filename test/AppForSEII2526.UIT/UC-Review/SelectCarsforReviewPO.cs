using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Review
{
    public class SelectCarsforReviewPO : PageObject
    {
        public SelectCarsforReviewPO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // ===== Locators =====
        private IWebElement InputManufacturer => _driver.FindElement(By.Id("inputManufacturer"));
        private IWebElement InputFuelType => _driver.FindElement(By.Id("inputFuelType"));
        private IWebElement SearchButton => _driver.FindElement(By.Id("searchCars"));

        private IReadOnlyCollection<IWebElement> AddButtons =>
            _driver.FindElements(By.CssSelector("button[id^='carToReview_']"));

        private IWebElement ContinueButton =>
            _driver.FindElement(By.Id("continueReviewButton"));

        // ===== Actions =====
        public void SearchCars(string manufacturer, string fuelType)
        {
            InputManufacturer.Clear();
            InputManufacturer.SendKeys(manufacturer);

            InputFuelType.Clear();
            InputFuelType.SendKeys(fuelType);

            SearchButton.Click();
        }

        public void AddCars(int id)
        {
            WaitForBeingClickable(By.Id("carToReview_" + id));
            _driver.FindElement(By.Id("carToReview_" + id)).Click();
        }

        public void RemoveCars(int id)
        {
            WaitForBeingClickable(By.Id("removeCar_" + id));
            _driver.FindElement(By.Id("removeCar_" + id)).Click();
        }

        public bool ReviewNotAvailable()
        {
            return _driver.FindElement(By.Id("continueReviewButton")).Displayed == false;
        }
    }
}
