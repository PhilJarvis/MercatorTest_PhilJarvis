using MercatorTest_PhilJarvis.Web.Shared;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Web.ClientPortal.Global
{
    public class Footer : AbstractPage
    {
        public Footer(IWebDriver driver)
            : base(driver, false)
        {
        }

        // Use FindsBy to find Footer page elements eg.
        [FindsBy(How = How.ClassName, Using = "footer-logo")]
        public IWebElement Logo { get; set; }
    }
}
