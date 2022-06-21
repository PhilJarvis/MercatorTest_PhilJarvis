using NUnit.Framework;
using System.ComponentModel;
using System.Configuration;

namespace MercatorTest_PhilJarvis.Bootstrap
{
    public sealed class ConfigurationData
    {
        private ConfigurationData()
        {
            Local = LocalConfig.Instance;
        }

        public LocalConfig Local { get; private set; }
        public string TestRunId { get; private set; }

        public long Timeout { get; private set; }

        public static ConfigurationData Create(string testRunId)
        {
            var config = new ConfigurationData();
            config.TestRunId = testRunId;
            return config;
        }

        public sealed class LocalConfig
        {
            private static readonly LocalConfig instance = new LocalConfig();
            private LocalConfig()
            {
                var webUrl = GetValueOrDefault<string>("webUrl");
                WebUrl = new Uri(webUrl);
                WebUrlQueryString = GetValueOrDefault<string>("webUrlQueryString");
                Browser = GetValueOrDefault<string>("browser");
                DownloadsDirectory = GetValueOrDefault<string>("downloadsDirectory");
            }
            public static LocalConfig Instance { get { return instance; } }
            public Uri WebUrl { get; private set; }
            public string WebUrlQueryString { get; private set; }
            public string Browser { get; private set; }
            public string DownloadsDirectory { get; private set; }
            public string ExtentReportPath { get; private set; }

            private T GetValueOrDefault<T>(string key, bool shouldCheckAppConfigOnly = false, T defaultValue = default(T))
            {
                string setting = null;
                if (!shouldCheckAppConfigOnly && TestContext.Parameters.Exists(key))
                {
                    setting = TestContext.Parameters[key];
                }
                else if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                {
                    setting = ConfigurationManager.AppSettings[key];
                }
                else
                {
                    return defaultValue;
                }

                var conv = TypeDescriptor.GetConverter(typeof(T));
                return (T)conv.ConvertFrom(setting);
            }

        }
    }

    public class AuthenticationDetail
    {
        public Uri SiteAddress { get; private set; }
        public AuthenticationDetail(Uri url)
        {
            SiteAddress = url;
        }
    }
}
