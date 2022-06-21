using OpenQA.Selenium;

namespace MercatorTest_PhilJarvis.Drivers
{
    public interface IDriver : IDisposable
    {
        IWebDriver Driver { get; }

        void Reset();

        void CloseWindow();
    }
}