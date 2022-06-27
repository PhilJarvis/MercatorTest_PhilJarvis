using OpenQA.Selenium;
using System;

namespace MercatorTest_PhilJarvis.Web.Shared.Selector.Ordering
{
    public interface IWebElement
    {
        Func<OpenQA.Selenium.IWebElement> Get(IWebDriver driver);
    }
}
