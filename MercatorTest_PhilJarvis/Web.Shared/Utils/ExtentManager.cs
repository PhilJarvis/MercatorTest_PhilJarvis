using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace MercatorTest_PhilJarvis.Web.Shared.Utils
{
    class ExtentManager
    {
        public static ExtentReports extent = new ExtentReports();
        public static readonly object synchroniser = new object();
        private static string path = ExtentReport.GetExtentReportPath();
        private static string reportpath = path + DateTime.Now.ToString() + ".html";
        private static string reportFilename = "ExecutionReport";

        public static AventStack.ExtentReports.ExtentReports createInstance()
        {
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(reportpath);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            htmlReporter.Config.DocumentTitle = reportFilename;
            htmlReporter.Config.EnableTimeline = true;
            htmlReporter.Config.ReportName = reportFilename;
            extent.AddSystemInfo("Operating System : ", Environment.OSVersion.ToString());
            extent.AddSystemInfo("Machine name : ", Environment.MachineName.ToString());
            extent.AnalysisStrategy = AnalysisStrategy.BDD;

            extent.AttachReporter(htmlReporter);
            return extent;
        }

    }
}
