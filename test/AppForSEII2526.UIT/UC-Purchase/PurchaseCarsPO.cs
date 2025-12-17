using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit.Abstractions;
using System;

namespace AppForSEII2526.UIT.PageObjects.Purchase
{
    public class PurchaseCarsPO
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;
        protected ITestOutputHelper _output;

        public PurchaseCarsPO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void AddFirstCar()
        {
            // Esperar primero a que la tabla aparezca
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));

            // Usamos una lógica de reintento: Busca Y Clickea en el mismo paso.
            // Si el elemento da error (Stale), el Wait lo vuelve a intentar automáticamente.
            _wait.Until(driver =>
            {
                try
                {
                    var addButton = driver.FindElement(By.XPath("//table/tbody/tr[1]//button[contains(text(),'Add')]"));

                    if (addButton.Displayed && addButton.Enabled)
                    {
                        addButton.Click();
                        return true; // Éxito, salimos del bucle
                    }
                    return false; // Todavía no está listo, reintentar
                }
                catch (StaleElementReferenceException)
                {
                    return false; // El elemento cambió, reintentar
                }
                catch (NoSuchElementException)
                {
                    return false; // No encontrado aún, reintentar
                }
            });
        }

        public void Continue()
        {
            // Aplicamos la misma lógica robusta para el botón Continuar
            _wait.Until(driver =>
            {
                try
                {
                    var continueButton = driver.FindElement(By.XPath("//button[contains(text(),'Continue')]"));

                    if (continueButton.Displayed && continueButton.Enabled)
                    {
                        continueButton.Click();
                        return true;
                    }
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }
    }
}