using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenQA.Selenium
{
    public static class IWebElementExtensions
    {
        public static List<string> GetClasses(this IWebElement element)
        {
            return element.GetAttribute("class").Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static bool HasClass(this IWebElement element, string name)
        {
            return element.GetClasses().Contains(name.ToLower());
        }

        public static IWebElement FindElementByMultipleChoices(this IWebElement el, params By[] byClausesArray)
        {
            foreach (var byClause in byClausesArray)
            {
                IList<IWebElement> elToFind = el.FindElements(byClause);
                if (elToFind.Count() > 0)
                {
                    return elToFind[0];
                }
            }

            throw new ArgumentException();
        }

        public static IList<IWebElement> FindElementsByMultipleChoices(this IWebElement el, params By[] byClausesArray)
        {
            foreach (var byClause in byClausesArray)
            {
                IList<IWebElement> elsToFind = el.FindElements(byClause);
                if (elsToFind.Count() > 0)
                {
                    return elsToFind;
                }
            }

            throw new ArgumentException();
        }
    }
}
