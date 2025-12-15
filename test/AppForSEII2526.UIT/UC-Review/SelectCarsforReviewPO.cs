using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Review
{
    public class SelectCarsForReviewPO : PageObject
    {
        private By inputManufacturer = By.Id("inputManufacturer");
        private By inputFuelType = By.Id("inputFuelType");
        private By buttonSearch = By.Id("searchCars");
        private By buttonContinue = By.Id("continueReviewButton");

        public SelectCarsForReviewPO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        public void SearchCars(string manufacturer, string fuelType)
        {
            WaitForBeingVisible(inputManufacturer);

            _driver.FindElement(inputManufacturer).Clear();
            _driver.FindElement(inputManufacturer).SendKeys(manufacturer);

            _driver.FindElement(inputFuelType).Clear();
            _driver.FindElement(inputFuelType).SendKeys(fuelType);

            _driver.FindElement(buttonSearch).Click();
        }

        public void AddCarToReview(int carId)
        {
            var button = By.Id($"carToReview_{carId}");
            WaitForBeingClickable(button);
            _driver.FindElement(button).Click();
        }

        public void RemoveCarFromReview(int carId)
        {
            var button = By.Id($"removeCar_{carId}");
            WaitForBeingClickable(button);
            _driver.FindElement(button).Click();
        }

        public bool ReviewNotAvailable()
        {
            return !_driver.FindElement(buttonContinue).Displayed;
        }

        public void Continue()
        {
            WaitForBeingClickable(buttonContinue);
            _driver.FindElement(buttonContinue).Click();
        }
    }
}
