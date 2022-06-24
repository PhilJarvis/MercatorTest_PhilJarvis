﻿using MercatorTest_PhilJarvis.Bootstrap;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Messaging;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public abstract class AbstractPage : IWaitable
    {
        protected static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        protected IWebDriver Driver { get; private set; }

        //protected IWebDriver GetRawDriver()
        //{
        //    var driver = Driver as OpenQA.Selenium.Internal.IWrapsDriver;

        //    if (driver == null)
        //    {
        //        return Driver;
        //    }

        //    return driver.WrappedDriver;
        //}

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

        protected void Click(IWebElement element, string errorMessage)
        {
            if(element == null)
            {
                throw new NullElementException("A clickable Element must be supplied");
            }
            else if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new AuditFailureException("A message must be supplied to audit failure conditions");
            }

            try
            {
                Logger.Debug("Clicking on element: {0}", element.Text);
                element.Click();
            }
            catch (Exception ex)
            {

                Logger.Error(ex, "{0} {1}{2}{3}", ex.Message, Environment.NewLine, Driver.Url);
                throw new Exception(errorMessage + ex.Message); 
            }
        }

        protected T Click<T>(IWebElement element, T expectedPage, string errorMessage, IWaitableStrategy waitStrategy) where T : class, IWaitable
        {
            try
            {
                Click(element, errorMessage, waitStrategy);
                
                if (expectedPage != null)
                {
                    new WaitablePageStrategy<T>(Driver, expectedPage, TimeSpan.FromSeconds(60));
                }
            }

            catch (Exception)
            {

                throw;
            }
            return expectedPage;
        }

        protected void Click(IWebElement element, string errorMessage, IWaitableStrategy waitStrategy)
        {
            if (element == null)
            {
                throw new NullElementException("A clickable Element must be supplied");
            }
            else if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new AuditFailureException("A message must be supplied to audit failure conditions");
            }

            try
            {
                element.Click();

                if(null != waitStrategy)
                {
                    // can be null, sometimes theres no need to slow down the page and wait for everything to finish
                    waitStrategy.Wait();
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "{0} {1}{2}{3}", ex.Message, Environment.NewLine, Driver.Url);
                throw new Exception(errorMessage + ex.Message);
            }
        }

        protected void RightClick(IWebElement element)
        {
            Actions builder = new Actions(Driver);
            builder.ContextClick(element).Build().Perform();
        }

        protected void Hover(IWebElement element)
        {
            Actions builder = new Actions(Driver);
            builder.MoveToElement(element).Build().Perform();
        }

        //protected TimeSpan GetTimeout()
        //{
        //    if (((RemoteWebDriver)GetRawDriver()).IsActionExecutor)
        //    {
        //        return Driver.Manage().Timeouts().AsynchronousJavaScript;
        //    }

        //    return TimeSpan.FromSeconds(ConfigurationData.LocalConfig.Instance.Timeout);
        //}

        [Serializable]
        public class NullElementException : Exception
        {
            public NullElementException(string message) : base(message) { }
        }

        public class AuditFailureException : Exception
        {
            public AuditFailureException(string message) : base(message) { }
        }
    }
}
