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
        private By InputManufacturerBy => By.Id("inputManufacturer");
        private By InputFuelTypeBy => By.Id("inputFuelType");
        private By SearchButtonBy => By.Id("searchCars");

        private By ContinueButtonBy => By.Id("continueReviewButton");

        private By AddCarButtonBy(int carId) => By.Id($"carToReview_{carId}");
        private By RemoveCarButtonBy(int carId) => By.Id($"removeCar_{carId}");

        // ===== Actions =====

        public void SearchCars(string manufacturer, string fuelType)
        {
            WaitForBeingClickable(InputManufacturerBy);
            _driver.FindElement(InputManufacturerBy).Clear();
            _driver.FindElement(InputManufacturerBy).SendKeys(manufacturer);

            _driver.FindElement(InputFuelTypeBy).Clear();
            _driver.FindElement(InputFuelTypeBy).SendKeys(fuelType);

            _driver.FindElement(SearchButtonBy).Click();
        }

        public bool HasCars()
        {
            return _driver.FindElements(By.CssSelector("button[id^='carToReview_']")).Count > 0;
        }

        public void AddCar(int carId)
        {
            WaitForBeingClickable(AddCarButtonBy(carId));
            _driver.FindElement(AddCarButtonBy(carId)).Click();
        }

        public void RemoveCar(int carId)
        {
            WaitForBeingClickable(RemoveCarButtonBy(carId));
            _driver.FindElement(RemoveCarButtonBy(carId)).Click();
        }

        public bool ReviewNotAvailable()
        {
            return !_driver.FindElement(ContinueButtonBy).Displayed;
        }

        public void Continue()
        {
            _driver.FindElement(ContinueButtonBy).Click();
        }
    }
}
