using MercatorTest_PhilJarvis.Steps;
using MercatorTest_PhilJarvis.Drivers;
using System.Text;
using BoDi;
using MercatorTest_PhilJarvis.Bootstrap;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using AventStack.ExtentReports.Gherkin.Model;
using MercatorTest_PhilJarvis.Web.Shared.Utils;
using MercatorTest_PhilJarvis.Web.Shared;
using NLog;
using NLog.Config;
using System;

namespace MercatorTest_PhilJarvis
{
    [Binding]
    public class Application : AbstractStep
    {
        public static readonly object synchronizer = new object();
        public static readonly string TestId = DateTime.Now.TimeOfDay.TotalSeconds.ToString();
        public static readonly DriverManager driverManager = new DriverManager();
        private static ConfigurationData config;

        private readonly ObjectContainer objectContainer;

        public Application(ObjectContainer container)
        {
            container.RegisterFactoryAs<IWebDriver>(() =>
            {
                lock (synchronizer)
                {
                    return driverManager.GetDriver(config.Local.Browser, config.Local.DownloadsDirectory);
                }
            });
            container.RegisterInstanceAs(new AuthenticationDetail(new Uri(config.Local.WebUrl,config.Local.WebUrlQueryString)));
            container.RegisterInstanceAs<DriverManager>(driverManager);
            //container.RegisterInstanceAs(config.Report);
            container.RegisterInstanceAs(config);

            objectContainer = container;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            driverManager.KillAllChromeDriver();
            if (config == null)
            {
                config = ConfigurationData.Create(TestId);
            }
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            // start the Feature up
            ExtentReport.startFeature(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            lock (synchronizer)
            {
                ExtentReport.startScenario(ScenarioContext.ScenarioInfo.Title, FeatureContext.FeatureInfo.Title);
            }
        }

        [BeforeStep]
        public void BeforeStep()
        {
            lock (synchronizer)
            { 
                //Placeholder for any necessary logging
            }
        }

        [AfterStep]
        public void AfterStep()
        {
            lock (synchronizer)
            {
                var stepInfo = ScenarioContext.StepContext.StepInfo;
                var stepType = ScenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

                if (ScenarioContext.TestError == null)
                {
                    if (stepType.ToLower() == "given")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<Given>(ScenarioContext.StepContext.StepInfo.Text);
                    else if (stepType.ToLower() == "when")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<When>(ScenarioContext.StepContext.StepInfo.Text);
                    else if (stepType.ToLower() == "then")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<Then>(ScenarioContext.StepContext.StepInfo.Text);
                    else if (stepType.ToLower() == "and")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<And>(ScenarioContext.StepContext.StepInfo.Text);
                }
                else if (ScenarioContext.TestError != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(ScenarioContext.ScenarioInfo.Title)
                        .Append(TestId);

                    var mediaEntity = ScreenCapture.CaptureScreenAndReturnFilename(objectContainer.Resolve<IWebDriver>(), ScenarioContext.ScenarioInfo.Title.Trim());

                    if (stepType.ToLower() == "given")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<Given>(ScenarioContext.StepContext.StepInfo.Text).Fail(ScenarioContext.TestError.Message, mediaEntity);
                    else if (stepType.ToLower() == "when")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<When>(ScenarioContext.StepContext.StepInfo.Text).Fail(ScenarioContext.TestError.Message, mediaEntity);
                    else if (stepType.ToLower() == "then")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<Then>(ScenarioContext.StepContext.StepInfo.Text).Fail(ScenarioContext.TestError.Message, mediaEntity);
                    else if (stepType.ToLower() == "and")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<And>(ScenarioContext.StepContext.StepInfo.Text).Fail(ScenarioContext.TestError.Message, mediaEntity);
                }
                else if (ScenarioContext.TestError != null)
                {
                    if (stepType.ToLower() == "given")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Stop Defination Pending");
                    else if (stepType.ToLower() == "when")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Stop Defination Pending");
                    else if (stepType.ToLower() == "then")
                        ExtentReport.getScenario(ScenarioContext.ScenarioInfo.Title).CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Stop Defination Pending");
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            lock (synchronizer)
            {
                // add extent reporting at a later date
                if (ScenarioContext.TestError != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(ScenarioContext.ScenarioInfo.Title)
                        .Append(TestId);

                    var sc = new ScreenCapture();
                    sc.SaveBrowserScreen(objectContainer.Resolve<IWebDriver>(), sb.ToString());

                    var driver = objectContainer.Resolve<IWebDriver>();
                    driverManager.DisposeDriver(driver);
                }

                try
                {
                    var driver = objectContainer.Resolve<IWebDriver>();
                    driverManager.ReturnToPool(driver);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            // Log feature complete in a log file - P Jarvis 21/06/2022
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            driverManager.Dispose();
            driverManager.KillAllChromeDriver();
            ExtentReport.flushReport();
        }
    }
}
