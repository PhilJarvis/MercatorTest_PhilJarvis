using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public abstract class AbstractPage<T> : AbstractPage where T : AbstractPage<T>
    {
        protected readonly IWebElement menu;

        public AbstractPage(IWebDriver driver, IWebElement menu) : base(driver, false)
        {
            this.menu = menu;
        }

        public virtual bool IsOpen()
        {
            return menu.FindElements(By.XPath("//*[@aria-expanded='true']")).Count == 1;
        }

        public virtual bool IsEnabled()
        {
            return menu.Enabled;
        }
    }
}
