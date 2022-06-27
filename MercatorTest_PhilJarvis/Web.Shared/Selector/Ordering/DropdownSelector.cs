using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Web.Shared.Selector.Ordering
{
    public class DropdownItemElementState
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }

    public interface IDropdownItemElementSelector
    {
        DropdownItemElementState State { get; }
        void Select();
    }

    public class DropdownItemElementSelector : AbstractPage, IDropdownItemElementSelector
    {
        private readonly OpenQA.Selenium.IWebElement element;
        private readonly string selectedItemValue;

        public DropdownItemElementSelector(IWebDriver driver, OpenQA.Selenium.IWebElement element, string selectedItemValue) : base(driver, false)
        {
            this.element = element;
            this.selectedItemValue = selectedItemValue;
        }

        public DropdownItemElementState State
        {
            get
            {
                return GetState();
            }
        }

        public void Select()
        {
            // Implement the IWrapable interface in order to get the GetTimeout functioning 
            Click(element, "Checkbox item cannot be clicked", WaitableScriptFactory.Get(Driver, GetTimeout(), WaitableScriptType.Angular));
        }

        private DropdownItemElementState GetState()
        {
            var state = new DropdownItemElementState();

           // state.Text = this.element.GetAttribute("text");
            //state.Value = element.GetAttribute("value");
            state.IsSelected = this.selectedItemValue == state.Value;

            return state;
        }
    }

    public class DropDownSelector : AbstractPage
    {
        private readonly OpenQA.Selenium.IWebElement element;
        private readonly IWebElement elementLocator;

        public DropDownSelector(IWebDriver driver, IWebElement locator) : base(driver, false)
        {
            this.elementLocator = locator;
            element = this.elementLocator.Get(driver)();
        }

        public List<IDropdownItemElementSelector> Items()
        {
            return GetItems();
        }

        public string SelectedValue
        {
            get
            {
                return this.element.GetAttribute("value");
            }
        }

        public List<IDropdownItemElementSelector> GetItems()
        {
            return this.SelectElement.FindElements(By.CssSelector("option")).Select(element => (IDropdownItemElementSelector)new DropdownItemElementSelector(Driver, element, SelectedValue)).ToList();
        }

        private OpenQA.Selenium.IWebElement SelectElement
        {
            get
            {
                return this.element.FindElement(By.CssSelector("select"));
            }

        }
    }

    public class DropDownElementLocator : IWebElement
    {
        private readonly string name;
        private readonly string description;
        private readonly string selector = "this is the css selector";

        public DropDownElementLocator(string name, string description)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public Func<OpenQA.Selenium.IWebElement> Get(IWebDriver driver)
        {
            var selector = string.Format(this.selector, name, description);
            return () => driver.FindElement(By.CssSelector(selector));
        }
    }
}
