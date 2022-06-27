using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Web.Shared.Selector.Ordering
{
    public class CheckboxItemElementState
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }

    public interface ICheckboxItemElementSelector
    {
        CheckboxItemElementState State { get; }
        void Select();
    }

    public class CheckboxItemElementSelector : AbstractPage, ICheckboxItemElementSelector
    {
        private readonly OpenQA.Selenium.IWebElement element;

        public CheckboxItemElementSelector(IWebDriver driver, OpenQA.Selenium.IWebElement element) : base(driver,false)
        {
            this.element = element;
        }

        public CheckboxItemElementState State
        {
            get
            {
                return GetState();
            }
        }

        public void Select()
        {
            //Click(this.CheckboxInputElement, "Checkbox item cannot be clicked", WaitableScriptFactory.Get(Driver, Logger, GetTimeout(), WaitableScriptType.Angular));
        }

        private CheckboxItemElementState GetState()
        {
            var state = new CheckboxItemElementState();
            state.Text = element.Text;

            var inputElement = CheckboxInputElement;
            state.Value = inputElement.GetAttribute("value");
            state.IsSelected = inputElement.Selected;

            return state;
        }

        private OpenQA.Selenium.IWebElement CheckboxInputElement
        {
            get
            {
                return this.element.FindElement(By.CssSelector("input"));
            }
        }
    }

    public class CheckboxSelector : AbstractPage
    {
        private readonly OpenQA.Selenium.IWebElement element;
        private readonly IWebElement elementLocator;
        private readonly RequiredSelector requiredSelector;

        public CheckboxSelector(IWebDriver driver, IWebElement locator) : base(driver, false)
        {
            elementLocator = locator;
            element = elementLocator.Get(driver)();
            requiredSelector = new RequiredSelector(element);
        }

        public RequiredSelector.RequiredElementState RequiredElementState
        {
            get
            {
                return requiredSelector.State;
            }
        }

        public List<ICheckboxItemElementSelector> Items
        {
            get
            {
                return GetItems();
            }
        }

        private List<ICheckboxItemElementSelector> GetItems()
        {
            return element.FindElements(By.CssSelector("div.checkbox")).Select(element => (ICheckboxItemElementSelector)new CheckboxItemElementSelector(Driver, element)).ToList();
        }
    }

    public class CheckboxElementLocator : IWebElement
    {
        private readonly string name;
        private readonly string description;
        private readonly string selector = "";

        public CheckboxElementLocator(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public Func<OpenQA.Selenium.IWebElement> Get(IWebDriver driver)
        {
            var selector = string.Format(this.selector, name, description);
            return () => driver.FindElement(By.CssSelector(selector));
        }

        public static List<CheckboxElementLocator> FindAll(IWebDriver driver)
        {
            var elementLocators = new List<CheckboxElementLocator>();
            var elements = driver.FindElements(By.CssSelector("FindTheLocatorInHtmlCode")).ToList();
            elements.ForEach(element =>
            {
                var name = element.GetAttribute("name");
                var description = element.FindElement(By.CssSelector("whatever")).GetAttribute("id");
                elementLocators.Add(new CheckboxElementLocator(name, description));
            });

            return elementLocators;
        }

        public string GetAttribute(string v)
        {
            throw new NotImplementedException();
        }
    }
}
