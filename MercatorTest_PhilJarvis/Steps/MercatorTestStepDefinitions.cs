using MercatorTest_PhilJarvis.Bootstrap;
using MercatorTest_PhilJarvis.Web.ClientPortal;
using TechTalk.SpecFlow;

namespace MercatorTest_PhilJarvis.Steps
{
    [Binding]
    public class MercatorTestStepDefinitions: AbstractStep
    {
        private readonly Entrance entrance;
        private readonly AuthenticationDetail authDetail;

        public MercatorTestStepDefinitions(Entrance entrance, AuthenticationDetail authDetail)
        {
            this.entrance = entrance;
            this.authDetail = authDetail;
        }

        [Given(@"The Site is available")]
        public void TheSiteIsAvailable()
        {
           var homepage = entrance.Login(authDetail.SiteAddress);
        }

        [Then(@"I click on the Dresses Menu item")]
        public void ThenIClickOnTheDressesMenuItem()
        {
            var homepage = entrance.OpenDressesMenu();
        }

        [Then(@"I select the highest price item")]
        public void ThenISelectTheHighestPriceItem()
        {
            var homepage = entrance.SelectTheHighestPriceItem();
        }

        [Then(@"I select the highest price item to the cart")]
        public void ThenISelectTheHighestPriceItemToTheCart()
        {
           var homepage = entrance.SendToShoppingCart();
        }
    }
}
