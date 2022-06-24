using OpenQA.Selenium;
using System;
using System.Diagnostics;

namespace MercatorTest_PhilJarvis.Drivers
{
    public abstract class AbstractDriver : IDriver
    {
        protected IWebDriver driver;
        protected DriverService driverService;

        public AbstractDriver()
        {
        }

        public virtual IWebDriver Driver
        {
            get
            {
                return driver;
            }
        }

        public void CloseWindow()
        {
            for (var index = 0; index < driver.WindowHandles.Count; index++)
            {
                driver.SwitchTo().Window(driver.WindowHandles[index]);
                driver.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(driver != null)
                {
                    var processId = driverService.ProcessId;
                    var isRunning = driverService.IsRunning;

                    try 
                    {
                        driver.Quit();
                    }
                    catch(Exception) 
                    {
                        // Log exception
                    }
                    finally 
                    { 
                        if(driver != null && isRunning)
                        {
                            driverService.Dispose();
                            var process = Process.GetProcessById(processId);
                            process.Kill();
                            process.WaitForExit();  
                        }
                    }
                }
            }
        }

        public void Reset()
        {
            for (var index = 1; index < driver.WindowHandles.Count; index++)
            {
                driver.SwitchTo().Window(driver.WindowHandles[index]);
                driver.Close();
            }
        }
    }
}
