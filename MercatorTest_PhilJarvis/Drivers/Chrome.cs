using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MercatorTest_PhilJarvis.Drivers
{
    public sealed class Chrome : AbstractDriver
    {
        public Chrome(string downloadDirectory)
        {
            Initialise(downloadDirectory, out driver, out driverService);
        }

        private void Initialise(string downloadDirectory, out IWebDriver driver, out DriverService driverService)
        {
            var chromeOptions = new ChromeOptions
            {
                LeaveBrowserRunning = false
            };

            chromeOptions.AddArgument("--start-maximized");

            driverService = ChromeDriverService.CreateDefaultService();
            driver = new ChromeDriver((ChromeDriverService)driverService, chromeOptions);
        }
    }
}
