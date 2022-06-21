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
   
    }
}
