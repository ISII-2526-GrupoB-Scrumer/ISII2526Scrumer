using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Review
{
    public class CUReview_UIT : UC_UIT
    {
        private SelectCarsforReviewPO selectCarsPO;

        public CUReview_UIT(ITestOutputHelper output) : base(output)
        {
        }

        

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_AF1_SelectCars_FilterAndSelect()
        {
           
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(_URI + "review/selectcarsforreview");


            // Act – filtrar
            selectCarsPO = new SelectCarsforReviewPO(_driver, _output);

            // Assert – hay coches
            selectCarsPO.AddCars(5);
            selectCarsPO.RemoveCars(5);

            Assert.True(selectCarsPO.ReviewNotAvailable());
        }
    }
}
