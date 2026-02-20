using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526
{
    public class CreateMaintenancePO : PageObject
    {
        private By inputNombre = By.Id("nombre");
        private By inputDireccion = By.Id("direccion");
        private By inputTelefono = By.Id("telefono");
        private By selectPago = By.Id("metodopago");
        private By inputComentario = By.Id("comentario");
        private By btnContratar = By.Id("contratar");

        public CreateMaintenancePO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillForm(string nombre, string direccion, string telefono, string pago, string comentario)
        {
            WaitForBeingVisible(inputNombre);

            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            _driver.FindElement(inputDireccion).Clear();
            _driver.FindElement(inputDireccion).SendKeys(direccion);

            _driver.FindElement(inputTelefono).Clear();
            _driver.FindElement(inputTelefono).SendKeys(telefono);

            new SelectElement(_driver.FindElement(selectPago)).SelectByText(pago);

            var comentarios = _driver.FindElements(inputComentario);
            foreach (var campo in comentarios)
            {
                campo.Clear();
                campo.SendKeys(comentario);
            }
        }

        public void ClickContratar()
        {
            WaitForBeingClickable(btnContratar);
            _driver.FindElement(btnContratar).Click();
        }

        // Método actualizado para detectar errores
        // CreateMaintenancePO.cs
        public bool ErrorVisible()
        {
            try
            {
                // 1. Busca el error manual (ErrorsShown) definido en el Razor
                bool manualError = _driver.FindElements(By.Id("ErrorsShown")).Any(e => e.Displayed);

                // 2. Busca los mensajes de validación automáticos de Blazor
                bool summaryError = _driver.FindElements(By.ClassName("validation-message")).Any(e => e.Displayed);

                return manualError || summaryError;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void FillComments(string comentario)
        {
       
            var camposComentario = _driver.FindElements(inputComentario);

            foreach (var campo in camposComentario)
            {
                campo.Clear();
                campo.SendKeys(comentario);
            }
        }
    }
}
