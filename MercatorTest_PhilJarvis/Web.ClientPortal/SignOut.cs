using MercatorTest_PhilJarvis.Web.Shared;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace MercatorTest_PhilJarvis.Web.ClientPortal
{
    public class SignOut : AbstractPage
    {
        public SignOut(IWebDriver driver, bool shouldUserRetryLocater) : base(driver, shouldUserRetryLocater)
        {
        }

        [FindsBy(How = How.XPath, Using = "SignOutLabel")]
        protected IWebElement SignOutLabel { get; set; }

        [FindsBy(How = How.XPath, Using = "Instruction")]
        protected IWebElement SignOutMessage { get; set; }

        public override bool IsReady()
        {
            return IsReady(SignOutMessage) && IsReady(SignOutLabel);
        }
    }
}
