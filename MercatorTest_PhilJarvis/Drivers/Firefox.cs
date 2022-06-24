using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;

namespace MercatorTest_PhilJarvis.Drivers
{
    public sealed class Firefox : AbstractDriver
    {
        public Firefox(string downloadsDirectory)
        {
            Initialise(downloadsDirectory, out driver, out driverService);
        }

        private void Initialise(string downloadsDirectory, out IWebDriver driver, out DriverService driverService)
        {
            var options = new FirefoxOptions
            {
                BrowserExecutableLocation = @"C:\Program Files (x86)\Nightly\firefox.exe"

            };

            options.SetPreference("browser.download.folderlist", 2);
            options.SetPreference("browser.download.manager.showWhenStarting", false);
            options.SetPreference("browser.download.dir", downloadsDirectory);
            options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/x-gzip");

            driverService = FirefoxDriverService.CreateDefaultService();
            driver = new FirefoxDriver((FirefoxDriverService)driverService, options,TimeSpan.FromMinutes(5));
            driver.Manage().Window.Maximize();
        }
    }
}
