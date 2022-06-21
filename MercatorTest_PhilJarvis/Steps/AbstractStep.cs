using MercatorTest_PhilJarvis.Bootstrap;
using MercatorTest_PhilJarvis.Web.Shared;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace MercatorTest_PhilJarvis.Steps
{
    public abstract class AbstractStep : TechTalk.SpecFlow.Steps
    {
        public AbstractStep()
        {

        }
        protected virtual void AssertReady(IWaitable expectedPage, string errorMessage)
        {
            Assert.That(expectedPage.IsReady(), errorMessage);
        }
    }
}
