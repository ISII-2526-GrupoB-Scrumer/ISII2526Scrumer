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

        // En DetailMaintenancePO.cs
        public bool CheckDetails(string nombre, string direccion, string precioTotal)
        {
            WaitForBeingVisible(By.Id("TotalPrice"));

            bool check = true;
            check &= _driver.FindElement(By.Id("ClientName")).Text.Contains(nombre);
            check &= _driver.FindElement(By.Id("ClientAddress")).Text.Contains(direccion);

            // Nueva validación del precio total
            if (!string.IsNullOrEmpty(precioTotal))
            {
                check &= _driver.FindElement(By.Id("TotalPrice")).Text.Contains(precioTotal);
            }

            return check;
        }

        // Nuevo método para verificar que un item específico existe en la tabla de detalles
        public bool IsServiceInTable(int reservaId)
        {
            try
            {
                return _driver.FindElement(By.Id($"Item_{reservaId}")).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}