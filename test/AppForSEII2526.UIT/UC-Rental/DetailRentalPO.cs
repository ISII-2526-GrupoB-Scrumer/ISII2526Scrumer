using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Rental
{
    public class DetailRentalPO : PageObject
    {
        public DetailRentalPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckRentalDetail(
            string name,
            string delivery,
            string paymentMethod,
            int totalPrice)
        {
            WaitForBeingVisible(By.Id("TotalPrice"));

            bool result = true;

            result &= _driver.FindElement(By.Id("NameSurname"))
                .Text.Contains(name);

            result &= _driver.FindElement(By.Id("DeliveryAddress"))
                .Text.Contains(delivery);

            result &= _driver.FindElement(By.Id("PaymentMethod"))
                .Text.Contains(paymentMethod);

            result &= _driver.FindElement(By.Id("TotalPrice"))
                .Text.Contains(totalPrice.ToString());

            return result;
        }

        public bool CheckListOfCars(List<string[]> expectedRentalItems)
        {
            return CheckBodyTable(expectedRentalItems, By.Id("RentedCars"));
        }

    }
}
