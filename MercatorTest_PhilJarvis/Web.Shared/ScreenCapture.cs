using AventStack.ExtentReports;
using NLog;
using NLog.Targets;
using OpenQA.Selenium;
using System;
using System.IO;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public sealed class ScreenCapture
    {

        public void SaveBrowserScreen(IWebDriver driver, string title)
        {
            try
            {
                var fileName = GetScreenshotFilename(title);
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile((string)fileName, ScreenshotImageFormat.Png);

            }
            catch (Exception)
            {
                // lOG FAILED TO TAKE SCREENSHOT
            }
        }

        public static MediaEntityModelProvider CaptureScreenAndReturnFilename(IWebDriver driver, string title)
        {
            try
            {
                var Screenshot = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
                return MediaEntityBuilder.CreateScreenCaptureFromPath(Screenshot, title).Build();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private object GetScreenshotFilename(string title)
        {
            var file = LogManager.Configuration.FindTargetByName("fileTarget") as FileTarget;
            var dir = Path.GetDirectoryName(file.FileName.Render(new LogEventInfo { TimeStamp = DateTime.Now }));
            title = String.Join(string.Empty, title.Split(Path.GetInvalidFileNameChars()));
            var date = DateTime.Now.ToLongTimeString().Replace(":", "");
            return Path.Combine(dir, title + "_" + date + ".png");
        }
    }
}
