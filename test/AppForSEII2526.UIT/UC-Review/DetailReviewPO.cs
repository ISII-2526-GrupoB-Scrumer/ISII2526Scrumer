using OpenQA.Selenium;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.UC_Review
{
    public class DetailReviewPO : PageObject
    {
        public DetailReviewPO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) { }

        public bool CheckReviewHeader(string name, string country, string driverType)
        {
            WaitForBeingVisible(By.Id("ReviewerName"));

            bool result = true;

            result &= _driver.FindElement(By.Id("ReviewerName"))
                .Text.Contains(name);

            result &= _driver.FindElement(By.Id("ReviewerCountry"))
                .Text.Contains(country);

            result &= _driver.FindElement(By.Id("ReviewerDriverType"))
                .Text.Contains(driverType);

            return result;
        }

        public bool CheckReviewedCars(List<string[]> expectedCars)
        {
            return CheckBodyTable(expectedCars, By.Id("ReviewedCars"));
        }
    }
}
