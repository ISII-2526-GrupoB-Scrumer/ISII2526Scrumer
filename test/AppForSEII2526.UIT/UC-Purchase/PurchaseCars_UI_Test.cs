using AppForSEII2526.UIT.PageObjects.Purchase;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class PurchaseCars_UI_Test : UC_UIT
    {
        private readonly WebDriverWait _wait;

        public PurchaseCars_UI_Test(ITestOutputHelper output)
            : base(output)
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }


        [Fact]
        public void UC1_6_EmptyDeliveryDealer_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            // Rellenar TODO menos delivery
            FillPurchaseForm(
                delivery: null,                 // ← el que queremos probar
                payment: "Visa",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            // Recuperar error de validación
            var error = _wait.Until(ExpectedConditions.ElementIsVisible(
    By.CssSelector("div.alert.alert-danger")
));

            Assert.Contains("Delivery location is required", error.Text);
        }


        // =====================================================
        // UC1_8 — Esc-5
        // Empty payment method
        // =====================================================
        [Fact]
        public void UC1_8_EmptyPaymentMethod_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: null,
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var error = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[contains(text(),'Payment')]")
            ));

            Assert.True(error.Displayed);
        }

        // =====================================================
        // UC1_9 — Esc-5
        // Empty driver type
        // =====================================================
        [Fact]
        public void UC1_9_EmptyDriverType_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Visa",
                driver: null,
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            var error = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[contains(text(),'Driver')]")
            ));

            Assert.True(error.Displayed);
        }

        // =====================================================
        // UC1_10 — Esc-5
        // Empty purchase date
        // =====================================================
        [Fact]
        public void UC1_10_EmptyPurchaseDate_ShowsError()
        {
            GoToCreatePurchaseWithOneCar();

            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Visa",
                driver: "Experienced",
                country: "Spain",
                date: null
            );

            var error = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[contains(text(),'Date')]")
            ));

            Assert.True(error.Displayed);
        }

        [Fact]
        public void UC1_VisaSuccessfulPurchase_FlowWorksCorrectly() //
        {
            // Abre la página de selección de coches
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Añadir coche al carrito
            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            // Completar el formulario (FillPurchaseForm YA HACE EL CLICK INTERNAMENTE)
            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Visa",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            // ❌ BORRA ESTA LÍNEA QUE CAUSA EL ERROR:
            // _driver.FindElement(By.XPath("//button[contains(text(),'Confirm Purchase')]")).Click();

            // Esperar a la redirección a la página de detalles de compra
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            // Verificar que la URL contenga "purchase/details/"
            Assert.Contains("/purchase/details/", _driver.Url);
        }

        [Fact]
        public void UC1_SuccessfulPurchase_WithMasterCard_FlowWorksCorrectly()
        {
            // Abre la página de selección de coches
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Añadir coche al carrito
            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            // Completar el formulario (FillPurchaseForm YA HACE EL CLICK DE CONFIRMAR)
            FillPurchaseForm(
                delivery: "Madrid Center",      // Dirección de entrega válida
                payment: "MasterCard",          // Método de pago válido (MasterCard)
                driver: "Experienced",          // Tipo de conductor válido
                country: "Spain",               // País válido
                date: DateTime.Now.AddDays(1)   // Fecha válida para la compra
            );

            // ❌ LÍNEA ELIMINADA:
            // _driver.FindElement(By.XPath("//button[contains(text(),'Confirm Purchase')]")).Click();

            // Esperar a la redirección a la página de detalles de compra
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            // Verificar que la URL contenga "purchase/details/"
            Assert.Contains("/purchase/details/", _driver.Url);
        }


        [Fact]
        public void UC1_SuccessfulPurchase_WithPayPal_FlowWorksCorrectly()
        {
            // Abre la página de selección de coches
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Añadir coche al carrito
            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            // Completar el formulario de compra con todos los datos correctos
            FillPurchaseForm(
                delivery: "Madrid Center",      // Dirección de entrega válida
                payment: "PayPal",              // Método de pago válido (PayPal)
                driver: "Experienced",          // Tipo de conductor válido
                country: "Spain",               // País válido
                date: DateTime.Now.AddDays(1)   // Fecha válida para la compra
            );

            // Confirmar la compra
            _driver.FindElement(By.XPath("//button[contains(text(),'Confirm Purchase')]")).Click();

            // Esperar a la redirección a la página de detalles de compra
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            // Verificar que la URL contenga "purchase/details/"
            Assert.Contains("/purchase/details/", _driver.Url);
        }


        
        [Fact]
        public void UC1_SuccessfulPurchase_WithBankTransfer_FlowWorksCorrectly()
        {
            // Abre la página de selección de coches
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Añadir coche al carrito
            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();

            // Completar el formulario y confirmar (FillPurchaseForm ya hace el Click)
            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Bank Transfer",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );

            // BORRA ESTA LÍNEA:
            // _driver.FindElement(By.XPath("//button[contains(text(),'Confirm Purchase')]")).Click();

            // Esperar a la redirección a la página de detalles de compra
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            // Verificar que la URL contenga "purchase/details/"
            Assert.Contains("/purchase/details/", _driver.Url);
        }


        [Fact]
        public void UC_FilterByColor_Rojo_DisplaysOnlyRojoCars()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']") // Suponiendo que el campo de color es un input
            ));
            colorInput.SendKeys("Rojo");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            var table = _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));

            // Aseguramos que solo se muestra el coche con color "Rojo"
            Assert.Contains("Rojo", table.Text);
            Assert.DoesNotContain("Blanco", table.Text);   // Aseguramos que "Blanco" no esté presente
            Assert.DoesNotContain("Azul", table.Text);     // Aseguramos que "Azul" no esté presente
        }


        [Fact]
        public void UC_FilterByColor_carritomodelocarritoeliminaelprimeroyterminalacompraDisplaysOnlyRojoCars()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']") // Suponiendo que el campo de color es un input
            ));
            colorInput.SendKeys("Blanco");

            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            var table = _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Tucson");
            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();
            var purchaseCars2 = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();
            Thread.Sleep(1000);
            _driver.FindElement(By.XPath("//button[contains(text(),'Modificar coches')]")).Click();
            purchaseCars.RemoveCarFromCart(5);
            purchaseCars.Continue();

            // Completar el formulario y confirmar (FillPurchaseForm ya hace el Click)
            FillPurchaseForm(
                delivery: "Madrid Center",
                payment: "Bank Transfer",
                driver: "Experienced",
                country: "Spain",
                date: DateTime.Now.AddDays(1)
            );
           
            // Esperar a la redirección a la página de detalles de compra
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("/purchase/details/"));

            // Verificar que la URL contenga "purchase/details/"
            Assert.Contains("/purchase/details/", _driver.Url);

            
        }


        [Fact]
        public void UC_FilterByModel_Ferrari_DisplaysNoResults()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Encuentra el campo de modelo e ingresa "Ferrari"
            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Ferrari");

            // Hacer clic en el botón de búsqueda
            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            // Esperar a que los resultados se actualicen
            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            // Aseguramos que no se muestra el modelo "Ferrari"
            Assert.DoesNotContain("Ferrari", table.Text);
            Assert.Contains("No results found", table.Text);  // Aseguramos que no hay resultados
        }

        [Fact]
        public void UC_FilterByColor_Morado_DisplaysNoResults()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Encuentra el campo de color e ingresa "Morado"
            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']")
            ));
            colorInput.SendKeys("Morado");

            // Hacer clic en el botón de búsqueda
            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            // Esperar a que los resultados se actualicen
            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            // Aseguramos que no se muestra el color "Morado"
            Assert.DoesNotContain("Morado", table.Text);
            Assert.Contains("No results found", table.Text);  // Aseguramos que no hay resultados
        }


        [Fact]
public void UC_FilterByModel_Civic_DisplaysOnlyCivic()
{
    Initial_step_opening_the_web_page();
    _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

    // Encuentra el campo de modelo e ingresa "Civic"
    var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
        By.CssSelector("input[placeholder='Model name']")
    ));
    modelInput.SendKeys("Civic");

    // Hacer clic en el botón de búsqueda
    _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

    // Esperar a que la página o la tabla se recargue completamente
    _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Search')]"))); // Esperamos que Search esté disponible nuevamente

            // Volver a encontrar la tabla después de que se haya actualizado
            var table = _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));

            // Verificar que solo el modelo "Civic" aparece en los resultados
            Assert.Contains("Civic", table.Text);
    Assert.DoesNotContain("Corrola", table.Text);   // Verificar que "Corrola" no esté presente
    Assert.DoesNotContain("Escape", table.Text);    // Verificar que "Escape" no esté presente
}

        [Fact]
        public void UC_FilterByModelAndColor_DisplaysOnlyMatchingCars()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Filtrar por Modelo: "Civic"
            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Civic");

            // Filtrar por Color: "Rojo"
            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']")
            ));
            colorInput.SendKeys("Rojo");

            // Hacer clic en el botón de búsqueda
            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            // Esperar a que los resultados se actualicen
            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            // Verificar que solo el coche con el modelo "Civic" y color "Rojo" aparece
            Assert.Contains("Civic", table.Text);
            Assert.Contains("Rojo", table.Text);
            Assert.DoesNotContain("Azul", table.Text);  // Aseguramos que "Azul" no esté presente
            Assert.DoesNotContain("Blanco", table.Text); // Aseguramos que "Blanco" no esté presente
        }

        [Fact]
        public void UC_FilterByModelAndColor_DisplaysNoResultsForColor()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            // Filtrar por Modelo: "Civic" (Debe devolver coches)
            var modelInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Model name']")
            ));
            modelInput.SendKeys("Civic");

            // Filtrar por Color: "Morado" (No debe devolver coches)
            var colorInput = _wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[placeholder='Color']")
            ));
            colorInput.SendKeys("Morado");

            // Hacer clic en el botón de búsqueda
            _driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();

            // Esperar a que los resultados se actualicen
            var table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            // Verificar que no hay resultados
            Assert.DoesNotContain("Civic", table.Text);  // Aseguramos que no se muestra el coche "Civic"
            Assert.Contains("No results found", table.Text); // Verificamos que se muestra un mensaje de "No results found"
        }




        // =====================================================
        // MÉTODOS AUXILIARES
        // =====================================================
        private void GoToCreatePurchaseWithOneCar()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "purchase/selectcarsforpurchase");

            var purchaseCars = new PurchaseCarsPO(_driver, _output);
            purchaseCars.AddFirstCar();
            purchaseCars.Continue();
        }

        private void FillPurchaseForm(
    string? delivery,  // Añade ? aquí
    string? payment,   // Añade ? aquí
    string? driver,    // Añade ? aquí
    string? country,   // Añade ? aquí
    DateTime? date)    // Este ya estaba bien
        {
            if (!string.IsNullOrEmpty(delivery))
                _driver.FindElement(By.CssSelector("input[placeholder='e.g. Madrid Center']"))
                       .SendKeys(delivery);

            var selects = _driver.FindElements(By.CssSelector("select.form-select"));

            if (!string.IsNullOrEmpty(payment))
                new SelectElement(selects[0]).SelectByText(payment);

            if (!string.IsNullOrEmpty(driver))
                new SelectElement(selects[1]).SelectByText(driver);

            if (!string.IsNullOrEmpty(country))
                _driver.FindElement(By.CssSelector("input[placeholder='e.g. Spain']"))
                       .SendKeys(country);

            if (date.HasValue)
                _driver.FindElement(By.CssSelector("input[type='date']"))
                       .SendKeys(date.Value.ToString("yyyy-MM-dd"));

            _driver.FindElement(By.XPath("//button[contains(text(),'Confirm')]")).Click();
        }
    }
}
