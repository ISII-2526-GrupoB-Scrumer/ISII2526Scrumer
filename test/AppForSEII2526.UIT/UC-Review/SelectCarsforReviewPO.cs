using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Review
{
    internal class SelectCarsforReviewPO : PageObject
    {
        // Constructor heredado de PageObject
        protected SelectCarsforReviewPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Locators de los elementos
        private IWebElement inputManufacturer => _driver.FindElement(By.Id("inputManufacturer"));
        private IWebElement inputFuelType => _driver.FindElement(By.Id("inputFuelType"));
        private IWebElement searchButton => _driver.FindElement(By.Id("searchCars"));
        private IReadOnlyList<IWebElement> carItems => _driver.FindElements(By.CssSelector(".car-item"));
        private IWebElement continueReviewButton => _driver.FindElement(By.Id("continueReviewButton"));

        // Método para buscar coches con filtros
        public void SearchCars(string manufacturer, string fuelType)
        {
            inputManufacturer.SendKeys(manufacturer);  // Escribe el fabricante en el campo
            inputFuelType.SendKeys(fuelType);          // Escribe el tipo de combustible
            searchButton.Click();                      // Da clic al botón de buscar coches
        }

        // Método para seleccionar un coche (por ejemplo, seleccionamos el primero)
        public void AddCarToReview(int carIndex)
        {
            if (carItems.Count > carIndex)
            {
                carItems[carIndex].Click(); // Da clic en el coche específico
            }
        }

        // Método para continuar al siguiente paso
        public void ContinueToCreateReview()
        {
            continueReviewButton.Click(); // Da clic en el botón de continuar
        }

        public void TestSelectCarForReview()
        {
            var reviewPage = new SelectCarsforReviewPO(_driver, _output);

            // Paso 1: Filtrar por fabricante y tipo de combustible
            reviewPage.SearchCars("Honda", "Gasolina");

            // Paso 2: Verificar que los resultados sean correctos (puedes añadir aserciones aquí)
            //Assert.IsTrue(reviewPage.carItems.Count > 0, "No cars found for the selected filters.");

            // Paso 3: Seleccionar el primer coche de la lista
            reviewPage.AddCarToReview(0);

            // Paso 4: Continuar al siguiente paso
            reviewPage.ContinueToCreateReview();
        }

    }
}