using MercatorTest_PhilJarvis.Web.Shared;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace MercatorTest_PhilJarvis.Web.ClientPortal.Global
{
    public partial class Header : AbstractPage
    {
        public Header(IWebDriver driver)
            : base(driver, false)
        {
        }

        // Grab all the Header page Elements etc
        [FindsBy(How = How.ClassName, Using = "Some Class for Logo etc")]
        protected IWebElement Logo { get; set; }

        [FindsBy(How = How.ClassName, Using = "Some Class for Help Dropdown etc")]
        protected IWebElement HelpDropdownDiv { get; set; }

        public override bool IsReady()
        {
            return IsReady(Logo);
        }

        public void WaitForHelpDropdown()
        {
            WaitForElement(HelpDropdownDiv, true, 10);
        }
    }
}
