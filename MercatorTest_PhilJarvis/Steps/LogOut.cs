using MercatorTest_PhilJarvis.Web.ClientPortal;
using MercatorTest_PhilJarvis.Web.ClientPortal.Global;
using TechTalk.SpecFlow;

namespace MercatorTest_PhilJarvis.Steps
{
    public class LogOut : AbstractStep
    {
        private readonly Header headerPage;
        private readonly SignOut signOutPage;

        public LogOut(Header headerPage, SignOut signOutPage)
        {
            this.headerPage = headerPage;
            this.signOutPage = signOutPage;
        }

        [Given(@"I Select The User Dropdown")]
        public void GivenISelecetTheUserDropdown()
        {
            //Assert.That(HeaderPage.)

        }

        [When(@"I Select The Sign Out")]
        public void WhenIselectTheSignOut()
        {

        }

        [Then(@"I will be signed out of the Application")]
        public void TheIWillBeSignedOutOfTheApplication()
        {

        }

    }
}
