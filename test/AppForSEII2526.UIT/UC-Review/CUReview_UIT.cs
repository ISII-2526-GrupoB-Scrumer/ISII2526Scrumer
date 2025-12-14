using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Review
{
    public class CUReview_UIT : UC_UIT
    {
        public CUReview_UIT(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_AF0_FilterCars()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var po = new SelectCarsforReviewPO(_driver, _output);

            po.SearchCars("Honda", "Gasolina");

            Assert.True(po.HasCars());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_AF1_NoCarsSelected_CannotContinue()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var po = new SelectCarsforReviewPO(_driver, _output);

            po.AddCar(5);
            po.RemoveCar(5);

            Assert.True(po.ReviewNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_AF2_RemoveSelectedCar()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");

            var po = new SelectCarsforReviewPO(_driver, _output);

            po.AddCar(5);
            po.RemoveCar(5);

            Assert.True(po.ReviewNotAvailable());
        }
    }
}
