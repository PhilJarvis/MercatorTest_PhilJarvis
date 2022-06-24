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
           entrance.Login(authDetail.SiteAddress);
           //Save("homepage", homepage);
        }

        [Then(@"I click on the Dresses Menu item")]
        public void ThenIClickOnTheDressesMenuItem()
        {
            entrance.OpenDressesMenu();
            //Save("homepage", homepage);
        }

        [Then(@"I select the highest price item")]
        public void ThenISelectTheHighestPriceItem()
        {
            entrance.SelectTheHighestPriceItem();
            //Save("homepage", homepage);
        }

        [Then(@"I switch to the popup Iframe")]
        public void ThenISwitchToThePopupIframe()
        {
            entrance.SwitchFrame();
           //ave("homepage", homepage);
        }

        [Then(@"I add the highest price item to the cart")]
        public void ThenIAddTheHighestPriceItemToTheCart()
        {
            entrance.AddToShoppingCart();
            //Save("homepage", homepage);
        }
    }
}
