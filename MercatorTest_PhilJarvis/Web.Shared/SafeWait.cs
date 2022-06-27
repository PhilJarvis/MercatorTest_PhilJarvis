using NLog;
using System;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public class SafeWait : IWaitable
    {
        protected static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly Func<bool> action;
        private readonly string message;

        public SafeWait(Func<bool> action, string message)
        {
            this.action = action;
            this.message = message;
        }
        public bool IsReady()
        {
            try
            {
                Logger.Debug("Waiting for {0}", message);
                return action();
            }
            catch (Exception ex)
            {
                Logger.Debug("Safe wait failed for {0} {1}", message, ex.Message);
                return false;
            }    
        }
    }
}
