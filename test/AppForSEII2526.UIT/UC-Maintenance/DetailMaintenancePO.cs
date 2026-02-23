using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Maintenance
{
    internal class DetailMaintenancePO : PageObject
    {
        public DetailMaintenancePO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckDetails(string nombre, string direccion, string precioTotal)
        {
            WaitForBeingVisible(By.Id("TotalPrice"));

            bool check = true;
            check &= _driver.FindElement(By.Id("ClientName")).Text.Contains(nombre); // [cite: 103]
            check &= _driver.FindElement(By.Id("ClientAddress")).Text.Contains(direccion); // [cite: 104]
            check &= _driver.FindElement(By.Id("TotalPrice")).Text.Contains(precioTotal); // [cite: 110]

            return check;
        }
    }
}
