using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AppForSEII2526.UIT.UC_Review
{
    public class CUReview_UIT : UC_UIT
    {
        private const int carId = 6; // Honda Civic

        public CUReview_UIT(ITestOutputHelper output) : base(output) { }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_01() // Reseña completa de un coche
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);
            var detailPO = new DetailReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: crear reseña
            createPO.FillReviewerData("Carlos", "carlitos_l", "España", "Experto");
            createPO.FillReviewItemByCarId(carId, "5", "Muy buen coche");

            createPO.SubmitReview();
            createPO.ConfirmDialog();

            // Esperar un momento para asegurar
            System.Threading.Thread.Sleep(1000);

            // Assert
            Assert.True(
                detailPO.CheckReviewHeader("Carlos", "España", "Experto")
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_02() // Filtrar coches por fabricante
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);

            // Act
            selectPO.SearchCars("Honda", "");

            // Esperar un momento para asegurar 
            System.Threading.Thread.Sleep(1000);

            // Assert
            Assert.True(
                selectPO.IsCarShownByModel("Civic"),
                "Honda Civic debería estar visible al filtrar por Honda"
            );

        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_03() // Filtrar coches por tipo de combustible
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);

            // Act
            selectPO.SearchCars("", "Gasolina");

            // Esperar un momento para asegurar
            System.Threading.Thread.Sleep(1000);

            // Assert
            Assert.True(
                selectPO.IsCarNotShownByModel("Leaf e+"),
                "Nissan Leaf e+ NO debería estar visible al filtrar por Gasolina"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_04() // Filtrar coches por fabricante y tipo de combustible
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);

            // Act
            selectPO.SearchCars("Honda", "Gasolina");

            // Esperar un momento para asegurar 
            System.Threading.Thread.Sleep(1000);

            // Assert
            Assert.True(
                selectPO.IsCarShownByModel("Civic"),
                "Honda Civic debería estar visible al filtrar por Honda y Gasolina"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_05() // Filtrar coches por fabricante y tipo de combustible sin resultados
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);

            // Act
            selectPO.SearchCars("Prueba", ""); // Fabricante que no existe

            // Esperar un momento para asegurar 
            System.Threading.Thread.Sleep(1000);

            // Assert
            Assert.True(
                selectPO.IsCarNotShownByModel("Civic"), // Verificamos que no aparece ningún Civic
                "No debería aparecer ningún coche al filtrar por fabricante Prueba"
            );
        }


        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_06() // No poder continuar sin coches seleccionados
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);

            // Esperar un momento para asegurar
            System.Threading.Thread.Sleep(1000);

            // Assert: Verificar que no se puede continuar sin coches seleccionados
            Assert.True(selectPO.ReviewNotAvailable());
        }


        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_07() // Quitar un coche seleccionado para reseña
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);

            // Act
            selectPO.AddCarToReview(carId);
            selectPO.RemoveCarFromReview(carId);

            // Esperar un momento para asegurar 
            System.Threading.Thread.Sleep(1000);

            // Assert
            Assert.True(
                selectPO.ReviewNotAvailable(),
                "No debería ser posible continuar sin coches seleccionados"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_08() // Intentar crear reseña con usuario inexistente
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: intentar crear reseña con usuario inexistente
            createPO.FillReviewerData("Usuario Prueba", "usuario_UC", "España", "Experto");
            createPO.FillReviewItemByCarId(carId, "5", "Muy buen coche");

            // No se puede enviar reseña
            createPO.SubmitReview();
            createPO.ConfirmDialog(); // Confirmar el cuadro de diálogo

            // Esperar un momento para asegurar que el error se muestre
            System.Threading.Thread.Sleep(1000);

            // Assert: verificar que aparece un error en el div con la clase .alert-danger
            var errorMessage = _driver.FindElement(By.CssSelector(".row.alert.alert-danger")).Text;

            // Verificar si el mensaje de error está contenido en el texto
            Assert.True(
                errorMessage.Contains("El usuario indicado no existe"),
                "Debería aparecer un mensaje de error si el usuario no existe"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_09() // Intentar crear reseña con valoración fuera de rango
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: intentar crear reseña con valoración fuera de rango (mayor a 5)
            createPO.FillReviewerData("Usuario Prueba", "usuario_UC", "España", "Experto");
            createPO.FillReviewItemByCarId(carId, "6", "Descripción errónea");

            // Hacer clic en el botón "Publicar reseñas"
            createPO.SubmitReview();

            // Esperar un momento para asegurar 
            System.Threading.Thread.Sleep(1000);

            // Assert: No debe aparecer la ventana de confirmación
            var modalHeader = _driver.FindElements(By.ClassName("modal-header"));
            Assert.Empty(modalHeader);
        }


        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_10() // Intentar crear reseña con descripción vacía
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);
            var detailPO = new DetailReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: intentar crear reseña con descripción vacía
            createPO.FillReviewerData("Carlos", "carlitos_l", "España", "Experto");
            createPO.FillReviewItemByCarId(carId, "5", "");  // Descripción vacía

            // Submit y verificar si aparece la ventana de confirmación
            createPO.SubmitReview();
            createPO.ConfirmDialog();

            // Esperar un momento para asegurar 
            System.Threading.Thread.Sleep(1000);

            // Assert: verificar que la descripción de la reseña no aparece en el detalle (ya que la descripción está vacía)
            Assert.True(
                detailPO.CheckReviewItemDescriptionIsEmpty(carId),
                "La descripción debería estar vacía en los detalles si la reseña no tiene descripción"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_11() // Falta de datos (Nombre)
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: intentar crear reseña sin completar todos los campos obligatorios
            createPO.FillReviewerData("", "carlitos_l", "España", "Experto"); // Sin nombre

            // Submit y verificar si aparece la ventana de confirmación
            createPO.SubmitReview();
            createPO.ConfirmDialog();

            // Esperar un momento para asegurar que el error se muestre
            System.Threading.Thread.Sleep(1000);

            // Assert: verificar que aparece un error en el div con la clase .alert-danger
            var errorMessage = _driver.FindElement(By.CssSelector(".row.alert.alert-danger")).Text;

            // Assert: verificar que aparece un mensaje de error
            Assert.True(
                errorMessage.Contains("Debes indicar tu nombre"),
                "Debería aparecer un mensaje de error si el nombre no existe"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_12() // Falta de datos (Nombre de usuario)
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: intentar crear reseña sin completar todos los campos obligatorios
            createPO.FillReviewerData("Carlos", "", "España", "Experto"); // Sin nombre de usuario

            // Submit y verificar si aparece la ventana de confirmación
            createPO.SubmitReview();
            createPO.ConfirmDialog();

            // Esperar un momento para asegurar que el error se muestre
            System.Threading.Thread.Sleep(1000);

            // Assert: verificar que aparece un error en el div con la clase .alert-danger
            var errorMessage = _driver.FindElement(By.CssSelector(".row.alert.alert-danger")).Text;

            // Assert: verificar que aparece un mensaje de error
            Assert.True(
                errorMessage.Contains("The ClientId field is required"),
                "Debería aparecer un mensaje de error si el nombre de usuario no existe"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_13() // Falta de datos (Pais)
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: intentar crear reseña sin completar todos los campos obligatorios
            createPO.FillReviewerData("Carlos", "carlitos_l", "", "Experto"); // Sin pais

            // Submit y verificar si aparece la ventana de confirmación
            createPO.SubmitReview();
            createPO.ConfirmDialog();

            // Esperar un momento para asegurar que el error se muestre
            System.Threading.Thread.Sleep(1000);

            // Assert: verificar que aparece un error en el div con la clase .alert-danger
            var errorMessage = _driver.FindElement(By.CssSelector(".row.alert.alert-danger")).Text;

            // Assert: verificar que aparece un mensaje de error
            Assert.True(
                errorMessage.Contains("Debes indicar tu país"),
                "Debería aparecer un mensaje de error si el pais no existe"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_14() // Falta de datos (Tipo de conductor)
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);

            // Act: seleccionar coche
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: intentar crear reseña sin completar todos los campos obligatorios
            createPO.FillReviewerData("Carlos", "carlitos_l", "España", ""); // Sin tipo de conductor

            // Submit y verificar si aparece la ventana de confirmación
            createPO.SubmitReview();
            createPO.ConfirmDialog();

            // Assert: verificar que aparece un error en el div con la clase .alert-danger
            var errorMessage = _driver.FindElement(By.CssSelector(".row.alert.alert-danger")).Text;

            // Esperar un momento para asegurar que el error se muestre
            System.Threading.Thread.Sleep(1000);

            // Assert: verificar que aparece un mensaje de error
            Assert.True(
                errorMessage.Contains("Debes indicar el tipo de conductor"),
                "Debería aparecer un mensaje de error si el  no existe"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_15() // Modificar coches seleccionados y completar reseña
        {
            // Arrange
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);
            var createPO = new CreateReviewPO(_driver, _output);
            var detailPO = new DetailReviewPO(_driver, _output);

            // Act: seleccionar coche inicial
            selectPO.AddCarToReview(7); 
            selectPO.Continue();

            // Act: comprobar que al volver los datos no desaparecen
            createPO.FillReviewerData("Carlos", "carlitos_l", "España", "Experto");

            // Act: volver atrás y seleccionar otro coche (carId)
            createPO.GoBackToSelectCars();
            selectPO.RemoveCarFromReview(7);
            selectPO.AddCarToReview(carId);
            selectPO.Continue();

            // Act: llenar la reseña y publicarla
            createPO.FillReviewItemByCarId(carId, "5", "Excelente coche");
            createPO.SubmitReview();
            createPO.ConfirmDialog();

            // Esperar un momento para asegurar 
            System.Threading.Thread.Sleep(1000);

            // Assert: verificar que el coche que aparece en los detalles es el Honda Civic (carId 6)
            Assert.True(
                detailPO.CheckCarModelInDetail(carId), 
                "El coche reseñado debe ser el Honda Civic"
            );
        }





    }
}
