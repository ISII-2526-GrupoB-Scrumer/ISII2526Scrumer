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

        public bool CheckReviewItemDescriptionIsEmpty(int carId)
        {
            try
            {
                var carRow = _driver.FindElement(By.Id($"ReviewItem_{carId}"));
                var descriptionCell = carRow.FindElement(By.XPath(".//td[5]")); // Descripción en la quinta columna
                return string.IsNullOrEmpty(descriptionCell.Text);
            }
            catch (NoSuchElementException)
            {
                return false; // Si no se encuentra el elemento, consideramos que la descripción no está vacía
            }
        }

        public bool CheckCarModelInDetail(int carId)
        {
            // Esperamos a que la tabla esté visible
            WaitForBeingVisible(By.Id("ReviewedCars"));

            // Obtenemos el nombre del modelo del coche basándonos en el carId
            string expectedModel = GetCarModelById(carId);

            // Buscamos la primera columna de la tabla donde está el modelo
            var modelCell = _driver.FindElement(By.XPath($"//table[@id='ReviewedCars']//tr/td[1]"));

            // Comparamos el modelo encontrado en la primera columna con el esperado
            string actualModel = modelCell.Text.Trim();

            return actualModel.Equals(expectedModel, StringComparison.OrdinalIgnoreCase);
        }

        // Método para obtener el modelo basado en el carId
        private string GetCarModelById(int carId)
        {
            switch (carId)
            {
                case 6: return "Civic";
                case 7: return "Escape";
            
                default: return "Desconocido";
            }
        }



    }
}
