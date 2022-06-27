using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public class ByLocalisationTag : By
    {
        // This function requires the new sites localise tag to work - just an example below - P Jarvis 27/06/2022
        protected ByLocalisationTag(string tag)
        {
            FindElementMethod = (ISearchContext context) =>
            {
                return context.FindElement(XPath("//*[@ng-localize='" + tag + "']"));
            };

            FindElementsMethod = (ISearchContext context) =>
            {
                return context.FindElements(XPath("//*[@ng-localize='" + tag + "']"));
            };
        }
    }
}
