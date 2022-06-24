using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;

namespace MercatorTest_PhilJarvis.Drivers
{
    public sealed class InternetExplorer :AbstractDriver
    {
        public InternetExplorer(string downloadsDirectory)
        {
            Initialise(downloadsDirectory, out driver, out driverService);
        }

        private void Initialise(string downloadsDirectory, out IWebDriver driver, out DriverService driverService)
        {
            var options = new InternetExplorerOptions();
            options.AddAdditionalCapability(CapabilityType.AcceptSslCertificates, true);

            driverService = InternetExplorerDriverService.CreateDefaultService();
            driver = new InternetExplorerDriver((InternetExplorerDriverService)driverService,options,TimeSpan.FromMinutes(5));
            driver.Manage().Window.Maximize();
        }
    }
}
