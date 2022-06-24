using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Extensions;
using System;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public interface IWaitableStrategy
    {
        void Wait();
    }

    public abstract class WaitableStrategy : IWaitableStrategy
    {
        protected readonly IWebDriver driver;
        protected readonly TimeSpan waitTimeout;
        protected readonly string waitReason;

        protected WaitableStrategy(IWebDriver driver, TimeSpan waitTimeout, string waitReason)
        {
            this.driver = driver;
            this.waitTimeout = waitTimeout;
            this.waitReason = waitReason;
        }

        public virtual void Wait()
        {
            var webDriverWait = new WebDriverWait(driver, waitTimeout);
            webDriverWait.Until(d => this.WaitCondition());

        }
        protected abstract bool WaitCondition();
    }

    public class WaitablePageStrategy<T> : WaitableStrategy where T : IWaitable
    {
        private readonly T page;

        public WaitablePageStrategy(IWebDriver driver, T page, TimeSpan waitTimeout) : base(driver, waitTimeout, typeof(T).Name)
        {
            this.page = page;
        }

        protected override bool WaitCondition()
        {
            return page.IsReady();
        }
    }

    public class WaitableScriptStrategy : WaitableStrategy
    {
        private readonly string script;

        public WaitableScriptStrategy(IWebDriver driver, string script, TimeSpan waitTimeout, string waitReason) : base(driver, waitTimeout, waitReason)
        {
            this.script = script;
        }

        protected override bool WaitCondition()
        {
            return this.driver.ExecuteJavaScript<bool>(script);
        }
    }

    public enum WaitableScriptType
    {
        None = 0,
        JQuery = 1,
        AngularJS = 2,
        Angular = 3,
        Custom = 4,
    }

    public class WaitableScriptFactory
    {
        public static IWaitableStrategy Get(IWebDriver driver, TimeSpan waitTimeout, WaitableScriptType waitType, string custom = "")
        {
            string script = string.Empty;
            switch (waitType)
            {
                default:
                case WaitableScriptType.None:
                    {
                        script = "return true";
                        break;
                    }
                case WaitableScriptType.Custom:
                    {
                        script = custom;
                        break;
                    }
                case WaitableScriptType.JQuery:
                    {
                        script = "return jQuery.active === 0";
                        break;
                    }
                case WaitableScriptType.AngularJS:
                    {
                        script = "return (window.angular !== undefined) && (angular..element(document).injector() !== undefined) && angular..element(document).injector().get('$http').pendingRequests.lemgth === 0)";
                        break;
                    }
                case WaitableScriptType.Angular:
                    {
                        script = "return window.getAllAngularTestabilities().every(function9testability) { return testability.isStable(); })";
                        break;
                    }
            }

            if (string.IsNullOrWhiteSpace(script))
            {
                {
                    throw new ArgumentException("Empty Script");
                }
            }

            return new WaitableScriptStrategy(driver, script, waitTimeout, Enum.GetName(typeof(WaitableScriptType), waitType));
        }
    }
}