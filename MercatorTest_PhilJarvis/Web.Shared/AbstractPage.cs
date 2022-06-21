using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public abstract class AbstractPage : IWaitable
    {
        protected IWebDriver Driver { get; private set; }

        protected AbstractPage(IWebDriver driver, bool shouldUserRetryLocater)
        {
            Driver = driver;
            if(shouldUserRetryLocater)
            {
                PageFactory.InitElements(Driver, this);
            }
        }

        protected virtual bool IsModalActive(string customClassName)
        {
            try
            {
                var isOpen = Driver.FindElement(By.ClassName(customClassName != null ? customClassName : "modal-open")) != null;
                return isOpen;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private IWebElement FindModal()
        {
            try
            {
                return Driver.FindElement(By.XPath("//*[@aria-hidden='false']"));
            }
            catch (Exception)
            {
                try
                {
                    return Driver.FindElement(By.XPath("//*[@modal-render='true']"));
                }
                catch (Exception)
                {
                    throw;
                }
               
            }
        }

        protected virtual IWebElement GetActiveModal()
        {
            try
            {
                var modal = FindModal(); 
            }
            catch (Exception)
            {
                throw;
            }

            return FindModal();
        }

        protected virtual bool IsReady(IWebElement element, bool preventrefresh = false)
        {
            if(element == null)
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
                wait.Message = "Waiting for Page readiness";
            }

            try
            {
                var result = element != null && element.Displayed && element.Enabled;
                return result;

            }
            catch (Exception)
            {
                return false;
            }
        }

        protected virtual bool IsElementAvailable(IWebElement element)
        {
            try
            {
                return element.Enabled && element.Displayed;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected virtual bool IsElementVisible(IWebElement element)
        {
            try
            {
                bool isVisible = element != null && element.Displayed;
                return isVisible;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool IsReady()
        {
            return true;
        }

        public string GetValueById(string value)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
            var toExecute = string.Format("return $('#{0}').val()", value);
            var toReturn = jse.ExecuteScript(toExecute);

            return toReturn.ToString();     
        }

        protected void Navigate(Uri address)
        {
            Driver.Navigate().GoToUrl(address);
        }

        protected T Click<T>(IWebElement element, T expectedPage, string errorMessage) where T : class, IWaitable
        {
            return Click(element, expectedPage, errorMessage);
        }
        

        protected void Click(IWebElement element, string errormessage)
        {
            if (element == null)
            {
                throw new NullElementException("A clickable Element must be supplied");
            }

            try
            {
                element.Click();
            }
            catch (Exception)
            {

                throw;
            }
          

        }
        [Serializable]
        public class NullElementException : Exception
        {
            public NullElementException(string message) : base(message) { }
        }
    }
}
