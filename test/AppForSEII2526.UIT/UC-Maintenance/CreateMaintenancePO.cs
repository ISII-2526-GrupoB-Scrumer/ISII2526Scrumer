using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526
{
    internal class CreateMaintenancePO : PageObject
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

            _driver.FindElement(inputNombre).SendKeys(nombre);
            _driver.FindElement(inputDireccion).SendKeys(direccion);
            _driver.FindElement(inputTelefono).SendKeys(telefono);

            
            new SelectElement(_driver.FindElement(selectPago)).SelectByText(pago);

            // Rellenar comentarios (si hay varios, Selenium los encuentra por el mismo ID)
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

        public bool ErrorVisible()
        {
            
            return _driver.FindElements(By.CssSelector(".alert-danger")).Count > 0;
        }
    }
}
