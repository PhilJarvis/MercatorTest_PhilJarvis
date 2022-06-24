using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Drivers
{
    public sealed class DriverManager : IDisposable
    {
        private readonly List<DriverInfo> driverManagerList;

        public DriverManager()
        {
            driverManagerList = new List<DriverInfo>(); 
        }

        public int DriverCount { get { return driverManagerList.Count; } }

        private IWebDriver CreateDriver(string browser, string downloadsDirectory)
        {
            var driverType = typeof(IDriver);

            var driverList = Assembly.GetExecutingAssembly().GetTypes().Where(i => string.Equals(i.Name, browser, StringComparison.InvariantCultureIgnoreCase) && driverType.IsAssignableFrom(i)).ToArray();

            if (driverList.Length != 1)
            {
                throw new Exception(String.Format("One Browser Type should be available.{0}", driverList.Length));
            }

            var newObj = Activator.CreateInstance(driverList.First(), downloadsDirectory);
            if (newObj == null)
            {
                throw new Exception("The web driver could not be instantiated");
            }

            var driverManger = newObj as IDriver;
            if(driverManger == null)
            {
                throw new Exception(String.Format("The driver should be of type.{0}", driverType));
            }

            driverManagerList.Add(new DriverInfo(driverManger,Thread.CurrentThread.ManagedThreadId) { IsActive=true});

            var timeoutSettings = driverManger.Driver.Manage().Timeouts();
            timeoutSettings.PageLoad = new TimeSpan(0, 1, 0);

            return driverManger.Driver;
        }

        public IWebDriver GetDriver(string browser, string downloadsDirectory)
        {
            var poolDriver = driverManagerList.FirstOrDefault(c => !c.IsActive && c.ThreadId == Thread.CurrentThread.ManagedThreadId); 
            if (poolDriver != null)
            {
                poolDriver.IsActive = true;
                return poolDriver.Driver;    
            }
            else
            {
                //Could not find a reusable driver for thread
            }

            return CreateDriver(browser, downloadsDirectory);

        }

        public void DisposeDriver(IWebDriver driver)
        {
            var driverFromPool = driverManagerList.FirstOrDefault(d => d.Driver == driver && d.ThreadId == Thread.CurrentThread.ManagedThreadId);
            if (driverFromPool != null)
            {
                try 
                {
                    driverFromPool.DriverManager.Dispose();
                    driverManagerList.Remove(driverFromPool);
                } 
                catch 
                { 
                    // Log failed to dispose of driver
                }
            }
        }

        public void KillAllChromeDriver()
        {
            try
            {
                Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
                foreach (var chromeProcess in chromeDriverProcesses)
                {
                    {
                        chromeProcess.Kill();
                    }
                }
            }
            catch
            {
                // Failed to kill driver off - log if neccessary
            }
            finally
            {
                Process.Start("CMD.exe", "/C taskkill /im chromedriver.exe /f /t");
                Process.Start("CMD.exe", "/C taskkill /im chrome.exe /f /t");
                Thread.Sleep(3000);
            }
        }

        public void ReturnToPool(IWebDriver driver)
        {
            var driverManger = driverManagerList.FirstOrDefault(d => d.Driver == driver && d.ThreadId == Thread.CurrentThread.ManagedThreadId);
            if (driverManger != null)
            {
                driverManger.DriverManager.Reset();
                driverManger.IsActive = false;
            }
            else
            {
                // The driver pool doesnt have a driver for this thread
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class DriverInfo
    {
        public DriverInfo(IDriver manager, int threadId)
        {
            DriverManager = manager;
            Driver = manager.Driver;
            ThreadId = threadId;
        }

        public bool IsActive { get; set; }

        public IDriver DriverManager { get; private set; }
        public int ThreadId { get; private set; }

        public IWebDriver Driver { get; private set; }
    }
}
