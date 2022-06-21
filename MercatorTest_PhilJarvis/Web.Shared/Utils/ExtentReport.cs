using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;

namespace MercatorTest_PhilJarvis.Web.Shared.Utils
{
    public class ExtentReport
    {
        public static readonly object synchroniser = new object();
        public static ExtentReports extentReports = ExtentManager.createInstance();
        private static ExtentTest extentTest = null;
        private static Dictionary<string, ExtentTest> extentFeatureMap = new Dictionary<string, ExtentTest>();
        private static Dictionary<string, ExtentTest> extentScenarioMap = new Dictionary<string, ExtentTest>();

        public static ExtentTest getFeature(string featureName)
        {
            lock (synchroniser)
            {
                return extentFeatureMap[featureName];   
            }
        }

        public static ExtentTest getScenario(string scenarioName)
        {
            lock (synchroniser)
            {
                try
                {
                    return extentScenarioMap[scenarioName];
                }
                catch
                {
                    return null;
                }
            }
        }

        public static ExtentTest startFeature(string featureName)
        {
            lock (synchroniser)
            {
                ExtentTest test;
                if (!extentFeatureMap.TryGetValue(featureName, out test))
                {
                    test = extentReports.CreateTest<Feature>(featureName);
                    extentFeatureMap.Add(featureName, test);
                }
                return test;
            }
        }

        public static ExtentTest startScenario(string scenarioName, string featureName)
        {
            lock (synchroniser)
            {
                if (getScenario(scenarioName) == null)
                {
                    extentTest = getFeature(featureName);
                    extentTest = extentTest.CreateNode<Scenario>(scenarioName);
                    extentScenarioMap.Add(scenarioName, extentTest);
                    return extentTest;
                }
                else
                {
                    return getScenario(scenarioName);
                }
            }
        }

        public static void flushReport()
        {
            extentReports.Flush();
        }

        public static T GetValueOrDefault<T>(string key,bool shouldCheckAppConfigOnly = false, T defaultvalue = default(T))
        {
            string setting = null;
            if(!shouldCheckAppConfigOnly && TestContext.Parameters.Exists(key))
            {
                setting = TestContext.Parameters[key];
            }
            else if(ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                setting = ConfigurationManager.AppSettings[key];
            }
            else 
            { 
                return defaultvalue;
            }

            var conv = TypeDescriptor.GetConverter(typeof(T));
            return (T)conv.ConvertFrom(setting);

        }

        public static string GetExtentReportPath()
        {
            var path = GetValueOrDefault<string>("extentReportPath");
            var url = GetValueOrDefault<Uri>("webUrl");
            //string environment;

            string extentReportPath = path + url;
            return path;
        }
    }
}
