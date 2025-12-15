using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Review
{
    public class CUReview_UIT : UC_UIT
    {
        private const int carId = 6; // Honda Civic

        public CUReview_UIT(ITestOutputHelper output) : base(output) { }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_CompleteReviewFlow()
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

            // Assert
            Assert.True(
                detailPO.CheckReviewHeader("Carlos", "España", "Experto")
            );
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_AF1_NoCarsSelected()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var selectPO = new SelectCarsForReviewPO(_driver, _output);

            selectPO.AddCarToReview(carId);
            selectPO.RemoveCarFromReview(carId);

            Assert.True(selectPO.ReviewNotAvailable());
        }
    }
}
