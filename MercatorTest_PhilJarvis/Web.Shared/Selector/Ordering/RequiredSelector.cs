using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercatorTest_PhilJarvis.Web.Shared.Selector.Ordering
{
    public class RequiredSelector
    {

        public class RequiredElementState
        {
            public bool IsRequired { get; set; }
            public long? MinAnswers { get; set; }
            public long? MaxAnswers { get; set; }
            public long? MaxNodes { get; set; }
        }

        private readonly OpenQA.Selenium.IWebElement element;
        private readonly Lazy<RequiredElementState> state;

        public RequiredSelector(OpenQA.Selenium.IWebElement element)
        {
            this.element = element;
            this.state = new Lazy<RequiredElementState>(() => GetState());
        }

        public RequiredElementState State
        {
            get
            {
                return this.state.Value;
            }
        }

        private RequiredElementState GetState()
        {
            var state = new RequiredElementState();
            var requiredAttribute = element.GetAttribute("prompt-required");
            if (null != requiredAttribute)
            {
                state.IsRequired = bool.Parse(requiredAttribute);
            }
            var minAnswersAttribute = element.GetAttribute("prompt-required-min-ans");
            if (null != minAnswersAttribute)
            {
                state.IsRequired = bool.Parse(minAnswersAttribute);
            }

            var maxAnswersAttribute = element.GetAttribute("prompt-required-max-ans");
            if (null != maxAnswersAttribute)
            {
                state.IsRequired = bool.Parse(maxAnswersAttribute);
            }

            var maxNodesAttribute = element.GetAttribute("prompt-required-max-nodes");
            if (null != maxNodesAttribute)
            {
                state.IsRequired = bool.Parse(maxNodesAttribute);
            }

            return state;
        }
    }
}
