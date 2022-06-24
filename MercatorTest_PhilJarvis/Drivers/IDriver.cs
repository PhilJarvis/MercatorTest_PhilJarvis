using OpenQA.Selenium;
using System;

namespace MercatorTest_PhilJarvis.Drivers
{
    public interface IDriver : IDisposable
    {
        IWebDriver Driver { get; }

        void Reset();

        void CloseWindow();
    }
}