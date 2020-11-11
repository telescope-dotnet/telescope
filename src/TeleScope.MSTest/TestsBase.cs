using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging;
using TeleScope.Logging.Extensions.Serilog;

namespace TeleScope.MSTest
{
    public abstract class TestsBase
    {

        // -- fields

        protected ILogger _log;
        protected const string SKIP_PLC_TESTS = "SkipPlcTests";

        // -- base methods

        public virtual void Arrange()
        {
            LoggingProvider.Initialize(
                 new LoggerFactory()
                     .UseTemplate("{Timestamp: HH:mm:ss} [{Level}] - {Message}{NewLine}{Exception}")
                     .UseLevel(LogLevel.Trace)
                     .AddSerilogConsole());
            _log = LoggingProvider.CreateLogger<TestsBase>();
        }

        public virtual void Cleanup()
        {

        }

        /// <summary>
        /// Helper method to throw specific exceptions and returns the properties
        /// </summary>
        /// <param name="context">The test context</param>
        /// <param name="key">The key to get the value of the property</param>
        /// <returns>The value of the property as a string.</returns>
        protected static string GetProperty(TestContext context, string key)
        {
            if (context.Properties[key] == null)
            {
                throw new ArgumentException($"The test property {key} does not exist, does not have a value, or a test setting is not selected");
            }
            else
            {
                return context.Properties[key].ToString();
            }
        }
    }
}