using MercatorTest_PhilJarvis.Bootstrap;
using System;
using MercatorTest_PhilJarvis.Web.Shared;
using NUnit.Framework;
using TechTalk.SpecFlow;
using NLog;

namespace MercatorTest_PhilJarvis.Steps
{
    [Binding]
    public abstract class AbstractStep : TechTalk.SpecFlow.Steps
    {
        protected static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public AbstractStep()
        {
        }
        protected virtual void AssertReady(IWaitable expectedPage, string errorMessage)
        {
            Assert.That(expectedPage.IsReady(), errorMessage);
        }

        protected void Store<T>(string key, T data)
        {
            Logger.Debug("Storing item {0}", key);

            if (ScenarioContext.ContainsKey(key))
            {
                throw new Exception(string.Format("Item already stored: {0}", key));
            }

            ScenarioContext.Set(data, key);
        }

        protected T Retrieve<T>(string key)
        {
            T result;

            if (ScenarioContext.TryGetValue(key, out result))
            {
                Logger.Debug("Found item {0}", key);
                return result;
            }

            throw new Exception(string.Format("Item not found: {0}", key));
        }

        protected T Replace<T>(string key, T data)
        {
            T result;

            if (ScenarioContext.TryGetValue(key, out result))
            {
                Logger.Debug("Found item {0}", key);
                ScenarioContext.Set(data, key);
                return data;
            }

            throw new Exception(string.Format("Item not found: {0}", key));
        }

        protected T Save<T>(string key, T data)
        {
            try
            {
                Store<T>(key, data);
            }
            catch (Exception)
            {
                Replace<T>(key, data);
            }

            return data;
        }
    }
}
