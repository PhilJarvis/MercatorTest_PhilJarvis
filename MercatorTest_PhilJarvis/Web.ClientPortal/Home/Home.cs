using MercatorTest_PhilJarvis.Web.Shared;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Web.ClientPortal.Home
{
    internal class Home : AbstractPage
    {
        public Home(IWebDriver driver, bool shouldUserRetryLocater) : base(driver, shouldUserRetryLocater)
        {
        }

        // Element selection code etc
        [FindsBy(How = How.Name, Using ="Title")]
        protected IWebElement Title { get; set; }

        public Home Refresh()
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
            var toExecute = "location.reload()";
            jse.ExecuteScript(toExecute);

            return new Home(Driver,true);   
        }
    }
}
